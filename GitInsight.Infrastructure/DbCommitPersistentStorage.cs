using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("GitInsight.Infrastructure.Tests")]
namespace GitInsight.Infrastructure;
internal class DbCommitPersistentStorage : ICommitPersistentStorage
{
    private PersistentStorageContext _context;
    public DbCommitPersistentStorage(PersistentStorageContext context)
    {
        _context = context;
    }

    public (string, Response) Create(DbCommitCreateDTO dbCommitCreate)
    {
        var entity = _context.Commits.FirstOrDefault(c => c.SHA == dbCommitCreate.SHA && c.RepoId == dbCommitCreate.RepoId);
        if(entity is not null) return (entity.SHA, Response.Conflict);
        _context.Commits.Add(new DbCommit {
            SHA = dbCommitCreate.SHA,
            AuthorName = dbCommitCreate.AuthorName,
            Date = dbCommitCreate.Date,
            RepoId = dbCommitCreate.RepoId
        });
        _context.SaveChanges();
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

    public (IReadOnlyCollection<DbCommitDTO>, Response) FindAllCommitsByRepoId(int repoId)
    {
        var commits = _context.Commits
            .OrderByDescending(dto => dto.Date)
            .Where(c => c.RepoId == repoId)
            .Select(dbCommit =>
            new DbCommitDTO(dbCommit.SHA, dbCommit.AuthorName, dbCommit.Date))
            .ToList()
            .AsReadOnly();
        
        return (commits, Response.Found);
    }

    public async Task<(DbCommitDTO?, Response)> FindAsync(string SHA)
    {
        var commit = await _context.Commits.FirstOrDefaultAsync(c => c.SHA == SHA);
        if(commit is null) return (null, Response.NotFound);
        return (new DbCommitDTO(commit.SHA, commit.AuthorName, commit.Date), Response.Found);
    }
}