namespace GitInsight.Infrastructure;

// should update newest commit

public class DbCommitPersistentStorage : ICommitPersistentStorage
{
    private PersistentStorageContext _context;
    public DbCommitPersistentStorage(PersistentStorageContext context)
    {
        _context = context;
    }

    public async Task<(string, Response)> CreateAsync(DbCommitCreateDTO dbCommitCreate)
    {
        var entity = await _context.Commits.FirstOrDefaultAsync(c => c.SHA == dbCommitCreate.SHA);
        if(entity is not null) return (entity.SHA, Response.Conflict);
        _context.Commits.Add(new DbCommit {
            SHA = dbCommitCreate.SHA,
            AuthorName = dbCommitCreate.AuthorName,
            Date = dbCommitCreate.Date,
            RepoId = dbCommitCreate.RepoId
        });
        await _context.SaveChangesAsync();
        return (dbCommitCreate.SHA, Response.Created);
    }

    public async Task<Response> DeleteAsync(string SHA)
    {
        var commit = await _context.Commits.FirstOrDefaultAsync(c => c.SHA == SHA);
        if (commit is null) return Response.NotFound;

        _context.Commits.Remove(commit);
        await _context.SaveChangesAsync();

        return Response.NoContent;
    }

    public async Task<(IReadOnlyCollection<DbCommitDTO>, Response)> FindAllCommitsByRepoIdAsync(int repoId)
    {
        var commits = _context.Commits.Where(c => c.RepoId == repoId).Select(dbCommit =>
            new DbCommitDTO(dbCommit.SHA, dbCommit.AuthorName, dbCommit.Date)).ToList().AsReadOnly();
        
        return (commits, Response.Found);
    }

    public async Task<(DbCommitDTO?, Response)> FindAsync(string SHA)
    {
        var commit = await _context.Commits.FirstOrDefaultAsync(c => c.SHA == SHA);
        if(commit is null) return (null, Response.NotFound);
        return (new DbCommitDTO(commit.SHA, commit.AuthorName, commit.Date), Response.Found);
    }
}