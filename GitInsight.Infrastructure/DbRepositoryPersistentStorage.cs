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

    public Task<(DbRepositoryDTO?, Response)> FindAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public Task<Response> UpdateAsync(DbRepositoryUpdateDTO dbRepositoryUpdateDTO)
    {
        throw new NotImplementedException();
    }
}
