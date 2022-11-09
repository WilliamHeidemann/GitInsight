namespace GitInsight.Infrastructure;

public class DbRepositoryPersistentStorage : IRepositoryPersistentStorage
{
    private PersistentStorageContext _context;
    public DbRepositoryPersistentStorage(PersistentStorageContext context)
    {
        _context = context;
    }

    public async Task<(int, Response)> CreateAsync(DbRepositoryCreateDTO dbRepositoryCreate)
    {
        var entity = await _context.Repositories.FirstOrDefaultAsync(t => t.FilePath == dbRepositoryCreate.Filepath);
        if(entity is not null) return (entity.Id, Response.Conflict);
        if (!Repository.IsValid(dbRepositoryCreate.Filepath)) return (-1, Response.BadRequest); //-1 because it is not a valid repo
        
        var realRepo = new Repository(dbRepositoryCreate.Filepath);
        var newestCommit = realRepo.Commits.FirstOrDefault();

        entity = new DbRepository(dbRepositoryCreate.Filepath);
        entity.NewestCommitSHA = newestCommit is not null ? newestCommit.Sha : null;

        _context.Repositories.Add(entity);
        await _context.SaveChangesAsync();

        var id = entity.Id;
        

        realRepo.Commits.ToList().ForEach(c => {
            _context.Commits.Add(new DbCommit {
                SHA = c.Sha,
                AuthorName = c.Committer.Name,
                Date = c.Committer.When.DateTime,
                RepoId = id
            });
        });

        await _context.SaveChangesAsync();

        return (id, Response.Created);
    }

    public async Task<(DbRepositoryDTO?, Response)> FindAsync(string filePath)
    {
        var repo = await _context.Repositories.FirstOrDefaultAsync(t => t.FilePath == filePath);
        if(repo is null) return (null, Response.NotFound);
        return (new DbRepositoryDTO(repo.Id, repo.FilePath, repo.NewestCommitSHA!), Response.Found);
    }

    public async Task<Response> UpdateAsync(DbRepositoryUpdateDTO dbRepositoryUpdateDTO)
    {
        var repo = await _context.Repositories.FirstOrDefaultAsync(t => t.FilePath == dbRepositoryUpdateDTO.FilePath);

        if(repo is null) return Response.NotFound;

        var realRepo = new Repository(dbRepositoryUpdateDTO.FilePath);

        realRepo.Commits.ToList().ForEach(c => {
            var commit = _context.Commits.FirstOrDefault(t => t.SHA == c.Sha);
            if (commit is null) {
                _context.Commits.Add(new DbCommit {
                SHA = c.Sha,
                AuthorName = c.Committer.Name,
                Date = c.Committer.When.DateTime,
                RepoId = dbRepositoryUpdateDTO.RepoId
            });
            }
        });
        _context.SaveChanges();
        repo.NewestCommitSHA = realRepo.Commits.FirstOrDefault()?.Sha;

        return Response.Updated;

    }
}
