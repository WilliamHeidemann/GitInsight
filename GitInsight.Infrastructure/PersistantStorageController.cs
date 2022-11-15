using GitInsight.Core;

namespace GitInsight;

public class PersistentStorageController 
{
    private readonly ICommitPersistentStorage _dbCommitPersistentStorage;
    private readonly IRepositoryPersistentStorage _dbRepositoryPersistentStorage;

    public PersistentStorageController(ICommitPersistentStorage dbCommitPersistentStorage, IRepositoryPersistentStorage dbRepositoryPersistentStorage)
    {
        _dbCommitPersistentStorage = dbCommitPersistentStorage;
        _dbRepositoryPersistentStorage = dbRepositoryPersistentStorage;
    }

    public async Task<IEnumerable<DbCommitDTO>> FindAllCommitsAsync(string filePath)
    {     
        if(!Repository.IsValid(filePath)) throw new RepositoryNotFoundException("The Repository does not exist. Please provide a valid filepath.");
        var (id, response) = await CreateRepoWithCommits(filePath);
        if(response == Response.Conflict) await UpdateRepoWithNewCommits(filePath);
        var (commits, findResponse) = _dbCommitPersistentStorage.FindAllCommitsByRepoId(id);
        return commits; 
    }

    public async Task<IEnumerable<CommitCountDTO>> GetFrequencyMode(string filePath) {
        var commits = await getCommits(filePath);

        return GroupCommitsByDate(commits);
    }

    public async Task<IEnumerable<AuthorCommitDTO>> GetAuthorMode(string filePath) {
        var commits = await getCommits(filePath);

        return GroupByAuthors(commits);
    }

    private IEnumerable<AuthorCommitDTO> GroupByAuthors(IEnumerable<DbCommitDTO> commits) {
        foreach (var authorcommits in commits
                     .GroupBy(commit => commit.AuthorName)
                     .Distinct())
        {
            yield return new(GroupCommitsByDate(authorcommits), authorcommits.First().AuthorName);
        }
    }

    private async Task<IReadOnlyCollection<DbCommitDTO>> getCommits(string filePath) {
        if(!Repository.IsValid(filePath)) throw new RepositoryNotFoundException("The Repository does not exist. Please provide a valid filepath.");
        var (id, response) = await CreateRepoWithCommits(filePath);
        if(response == Response.Conflict) await UpdateRepoWithNewCommits(filePath);
        var (commits, findResponse) = _dbCommitPersistentStorage.FindAllCommitsByRepoId(id);
        return commits;
    }

    private IEnumerable<CommitCountDTO> GroupCommitsByDate(IEnumerable<DbCommitDTO> commits) {
        return from commit in commits
            .GroupBy(commit => commit.Date.Date)
            let count = commit.Count()
            let date = commit.First().Date
            select new CommitCountDTO(count, date);
    }

    public async Task<IEnumerable<DbCommitDTO>> FindAllGithubCommits(string organizationName, string repositoryName) {
        var clonedRepoPath = $"../../Source/{organizationName}-{repositoryName}";
        var remoteRepository = $"https://github.com/{organizationName}/{repositoryName}";
        if (Repository.IsValid(clonedRepoPath)) {
            var repo = new Repository(clonedRepoPath);
            repo.Network.Fetch("origin", new List<string>(){"main"});
        } else {
            Repository.Clone(remoteRepository, clonedRepoPath);
        }
        
        return await FindAllCommitsAsync(clonedRepoPath);
    }

    private async Task<(int, Response)> CreateRepoWithCommits(string filePath)
    {
        var (id, response) = await _dbRepositoryPersistentStorage.CreateAsync(new DbRepositoryCreateDTO(filePath));
        if (response == Response.Conflict) return (id, Response.Conflict);
        var realRepo = new Repository(filePath);

        realRepo.Commits.ToList().ForEach(c =>
        {
            // should we add them by using the createAsync 
            _dbCommitPersistentStorage.Create(new DbCommitCreateDTO(c.Sha, c.Committer.Name, c.Committer.When.DateTime, id));
        });
        await UpdateRepoNewestSHA(id, realRepo);

        return (id, response);
    }

    private async Task UpdateRepoNewestSHA(int id, Repository realRepo)
    {
        var newestCommit = realRepo.Commits.FirstOrDefault();
        if (newestCommit is not null)
        {
            await _dbRepositoryPersistentStorage.UpdateNewestCommitSHA(newestCommit.Sha, id);
        }
    }

    private async Task<Response> UpdateRepoWithNewCommits(string filePath) 
    {
        var (repoDTO, response) = await _dbRepositoryPersistentStorage.FindAsync(filePath);

        if(response == Response.NotFound) return Response.NotFound;

        var realRepo = new Repository(filePath);

        realRepo.Commits.ToList().ForEach(c => {
             _dbCommitPersistentStorage.Create(new DbCommitCreateDTO(c.Sha, c.Committer.Name, c.Committer.When.DateTime, repoDTO!.RepoId));
        });
        await UpdateRepoNewestSHA(repoDTO!.RepoId, realRepo);

        return Response.Updated;

    }
}