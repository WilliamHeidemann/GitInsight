namespace GitInsight.Infrastructure;

public class PersistentStorage : IPersistentStorage
{
    private PersistentStorageContext _context;
    public PersistentStorage(PersistentStorageContext context)
    {
        _context = context;
    }
    public IEnumerable<DbCommitDTO> FindAllCommits(string filePath)
    {
        if(!Repository.IsValid(filePath)) throw new RepositoryNotFoundException("The Repository does not exist!");
        var (response, newestCommit) = FindNewestCommit(filePath);
        if (response == Response.NotFound)
        {
            Create(new DbRepositoryCreateDTO(filePath));
        }
        if (!IsUpToDate(filePath, newestCommit)) 
        {
            Update(filePath);
        }
        (_, newestCommit) = FindNewestCommit(filePath);
        return FindAllCommitsFromNewestCommit(newestCommit);
    }
    private IEnumerable<DbCommitDTO> FindAllCommitsFromNewestCommit(DbCommit? newestCommit)
    {
        while (newestCommit is not null)
        {
            yield return new DbCommitDTO(newestCommit.AuthorName, newestCommit.Date);
            newestCommit = newestCommit.ParentCommit;
        }
    }
    public Response Create(DbRepositoryCreateDTO dbRepositoryCreate)
    {
        var repo = _context.Repositories.FirstOrDefault(t => t.FilePath == dbRepositoryCreate.Filepath);
        if (repo is not null) return Response.Conflict;
        if(!Repository.IsValid(dbRepositoryCreate.Filepath)) return Response.BadRequest;
        var realNewestCommit = new Repository(dbRepositoryCreate.Filepath).Commits.FirstOrDefault();
        var newestCommit = realNewestCommit is null ? null : CreateDbCommit(realNewestCommit);
        var newRepo = new DbRepository
        {
            FilePath = dbRepositoryCreate.Filepath,
            NewestCommit = newestCommit,
        };
        _context.Add(newRepo);
        _context.SaveChanges();
        return Response.Created;
    }
    private DbCommit? CreateDbCommit(Commit realNewestCommit) => new()
    {
        SHA = realNewestCommit.Sha,
        AuthorName = realNewestCommit.Committer.Name,
        Date = realNewestCommit.Committer.When.Date,
        ParentCommit = realNewestCommit.Parents.FirstOrDefault() is not null ? CreateDbCommit(realNewestCommit.Parents.FirstOrDefault()!) : null
    };
    public (Response, DbCommit?) FindNewestCommit(string filepath)
    {
        var repo = _context.Repositories.Find(filepath);
        return repo is null ? (Response.NotFound, null) : (Response.Found, repo.NewestCommit);
    }
    private bool IsUpToDate(string filePath, DbCommit? newestCommitDb)
    {
        var repo = new Repository(filePath);
        var newestCommit = repo.Commits.FirstOrDefault();
        if (newestCommit is null != newestCommitDb is null) return false;
        return newestCommitDb?.SHA == newestCommit?.Sha;
    }
    public Response Update(string filePath)
    {
        var response = Delete(filePath);
        if (response == Response.NotFound) return response;
        Create(new DbRepositoryCreateDTO(filePath));
        return Response.Updated;
    }
    public Response Delete(string filePath) 
    {
        var repo = _context.Repositories.Find(filePath);
        if (repo is null) return Response.NotFound;
        _context.Repositories.Remove(repo);
        _context.SaveChanges();
        return Response.NoContent;
    }
}