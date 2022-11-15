namespace GitInsight.Infrastructure;

public class DbRepositoryPersistentStorage : IRepositoryPersistentStorage
{
    private PersistentStorageContext _context;
    private DbCommitPersistentStorage _dbCommitPersistentStorage;
    public DbRepositoryPersistentStorage(PersistentStorageContext context)
    {
        _context = context;
        _dbCommitPersistentStorage = new DbCommitPersistentStorage(_context);
    }

    public async Task<(int, Response)> CreateAsync(DbRepositoryCreateDTO dbRepositoryCreate)
    {
        var entity = await _context.Repositories.FirstOrDefaultAsync(t => t.FilePath == dbRepositoryCreate.Filepath);
        if(entity is not null) return (entity.Id, Response.Conflict);
        if (!LibGit2Sharp.Repository.IsValid(dbRepositoryCreate.Filepath)) return (-1, Response.BadRequest); //-1 because it is not a valid repo
        
        var realRepo = new LibGit2Sharp.Repository(dbRepositoryCreate.Filepath);
        var newestCommit = realRepo.Commits.FirstOrDefault();

        entity = new DbRepository(dbRepositoryCreate.Filepath);
        entity.NewestCommitSHA = newestCommit is not null ? newestCommit.Sha : null;

        _context.Repositories.Add(entity);
        await _context.SaveChangesAsync();

        var id = entity.Id;
        
        realRepo.Commits.ToList().ForEach(async c => {
            // should we add them by using the createAsync 
            await _dbCommitPersistentStorage.CreateAsync(new DbCommitCreateDTO(c.Sha, c.Committer.Name, c.Committer.When.DateTime, id));
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

        var realRepo = new LibGit2Sharp.Repository(dbRepositoryUpdateDTO.FilePath);
        var newestCommit = realRepo.Commits.FirstOrDefault();
        var newestCommitSHA = newestCommit is not null ? newestCommit.Sha : null;
        if(newestCommitSHA == repo.NewestCommitSHA) return Response.Updated;

        realRepo.Commits.ToList().ForEach(async c => {
            await _dbCommitPersistentStorage.CreateAsync(new DbCommitCreateDTO(c.Sha, c.Committer.Name, c.Committer.When.DateTime, repo.Id));
        });
        _context.SaveChanges();
        repo.NewestCommitSHA = realRepo.Commits.FirstOrDefault()?.Sha;

        return Response.Updated;

    }
}
