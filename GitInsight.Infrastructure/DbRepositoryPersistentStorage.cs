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
        if (!LibGit2Sharp.Repository.IsValid(dbRepositoryCreate.Filepath)) return (-1, Response.BadRequest); //-1 because it is not a valid repo
        entity = new DbRepository(dbRepositoryCreate.Filepath)
        {
            NewestCommitSHA = null // should be set later
        };

        _context.Repositories.Add(entity);
    
        await _context.SaveChangesAsync();

        return (entity.Id, Response.Created);
    }

    public async Task<(DbRepositoryDTO?, Response)> FindAsync(string filePath)
    {
        var repo = await _context.Repositories.FirstOrDefaultAsync(t => t.FilePath == filePath);
        if(repo is null) return (null, Response.NotFound);
        return (new DbRepositoryDTO(repo.Id, repo.FilePath, repo.NewestCommitSHA!), Response.Found);
    }

    public async Task UpdateNewestCommitSHA(string SHA, int repoId) {
        var repo = await _context.Repositories.FirstOrDefaultAsync(t => t.Id == repoId);
        repo!.NewestCommitSHA = SHA;
        await _context.SaveChangesAsync();
    }
}
