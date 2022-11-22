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

    public Results<Created<DbCommitCreateDTO>, Conflict<string>> Create(DbCommitCreateDTO dbCommitCreate)
    {
        var entity = _context.Commits.FirstOrDefault(c => c.SHA == dbCommitCreate.SHA && c.RepoId == dbCommitCreate.RepoId);
        if(entity is not null) 
        {
            return TypedResults.Conflict(entity.SHA);
        }
            entity = new DbCommit 
            {
                SHA = dbCommitCreate.SHA,
                AuthorName = dbCommitCreate.AuthorName,
                Date = dbCommitCreate.Date,
                RepoId = dbCommitCreate.RepoId
            };
            _context.Commits.Add(entity);
            _context.SaveChanges();
            return TypedResults.Created($"{entity.SHA}", dbCommitCreate with { SHA = entity.SHA });
    }

    public async Task<Results<NoContent, NotFound<string>>> DeleteAsync(string SHA)
    {
        var commit = await _context.Commits.FirstOrDefaultAsync(c => c.SHA == SHA);
        if (commit is null) return TypedResults.NotFound(SHA);

        _context.Commits.Remove(commit);
        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public Results<Ok<IReadOnlyCollection<DbCommitDTO>>, NotFound<int>> FindAllCommitsByRepoId(int repoId)
    {
        var repo = _context.Repositories.FirstOrDefaultAsync(r => r.Id == repoId);
        if (repo is null) return TypedResults.NotFound(repoId);
        var commits = _context.Commits
            .OrderByDescending(dto => dto.Date)
            .Where(c => c.RepoId == repoId)
            .Select(dbCommit => new DbCommitDTO(dbCommit.SHA, dbCommit.AuthorName, dbCommit.Date))
            .ToList()
            .AsReadOnly();

        return TypedResults.Ok<IReadOnlyCollection<DbCommitDTO>>(commits);
    }

    public async Task<Results<Ok<DbCommitDTO>, NotFound<string>>> FindAsync(string SHA)
    {
        var commit = await _context.Commits.FirstOrDefaultAsync(c => c.SHA == SHA);
        if(commit is null) return TypedResults.NotFound(SHA);
        return TypedResults.Ok(new DbCommitDTO(commit.SHA, commit.AuthorName, commit.Date));
    }
}