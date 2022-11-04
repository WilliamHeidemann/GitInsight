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
        var (response, newestCommit) = FindNewestCommit(filePath);
        if (response == Response.NotFound)
        {
            Create(new DbRepositoryCreateDTO(filePath));
        }
        if (!IsUpToDate(filePath, newestCommit)) 
        {
            Update(filePath);
        }
        
        return FindAllCommitsFromNewestCommit(FindNewestCommit(filePath).Item2);
    }

    public IEnumerable<DbCommitDTO> FindAllCommitsFromNewestCommit(DbCommit? newestCommit)
    {
        if(newestCommit is null) {
            yield return null!;
            yield break;
        }
        DbCommit? nextCommit = newestCommit.ParentCommit;
        while (newestCommit is not null) 
        {
            yield return new DbCommitDTO(newestCommit.AuthorName, newestCommit.Date);
            newestCommit = nextCommit;
            nextCommit = newestCommit!.ParentCommit;
        }
    }

    public Response Create(DbRepositoryCreateDTO dbRepositoryCreate)
    {
        var repo = _context.Repositories.FirstOrDefault(t => t.FilePath == dbRepositoryCreate.Filepath);

        if (repo is not null)
        {
            return Response.Conflict;
        }

        var realNewestCommit = new Repository(dbRepositoryCreate.Filepath).Commits.FirstOrDefault();
        DbCommit? newestCommit;
        if(realNewestCommit is null) newestCommit = null;
        else newestCommit = CreateDbCommit(realNewestCommit);

        var newRepo = new DbRepository
        {
            FilePath = dbRepositoryCreate.Filepath,
            NewestCommit = newestCommit,
        };

        _context.Add(newRepo);
        _context.SaveChanges();
        return Response.Created;
    }

    private DbCommit? CreateDbCommit(Commit realNewestCommit)
    {
        return new DbCommit
        {
            SHA = realNewestCommit.Sha,
            AuthorName = realNewestCommit.Committer.Name,
            Date = realNewestCommit.Committer.When.Date,
            ParentCommit = realNewestCommit.Parents.FirstOrDefault() is not null ? CreateDbCommit(realNewestCommit.Parents.FirstOrDefault()!) : null
        };

    }

    public (Response, DbCommit?) FindNewestCommit(string Filepath)
    {
        var repo = _context.Repositories.Find(Filepath);

        if (repo is null)
        {
            return (Response.NotFound, null);
        }
        else
        {
            if (repo.NewestCommit is null)
            {
                return (Response.Found, null);
            }
            var commit = _context.Commits.Find(repo.NewestCommit.SHA); // We need to test if repo.NewestCommit is null
            if (commit is null)
            {
                return (Response.Found, null);
            }
            else
            {
                return (Response.Found, commit);
            }
        }
    }

    public bool IsUpToDate(string filePath, DbCommit? newestCommitDb)
    {
        var repo = new Repository(filePath);
        Commit? newestCommit = FindRealNewestCommit(repo);
        if (newestCommit is null && newestCommitDb is null) return true;
        if (newestCommit is null || newestCommitDb is null) return false;
        if (newestCommitDb.SHA == newestCommit.Sha) return true;

        return false;
    }

    public Commit? FindRealNewestCommit(Repository repo) => repo.Commits.FirstOrDefault();

    public Response Update(string filePath)
    {
        var repo = _context.Repositories.Find(filePath);

        if (repo is null) return Response.NotFound;

        _context.Repositories.Remove(repo);
        _context.SaveChanges();

        Create(new DbRepositoryCreateDTO(filePath));

        return Response.Updated;
    }
}