namespace GitInsight.Infrastructure;

public class PersistentStorage : IPersistentStorage
{
    private PersistentStorageContext _context;
    public PersistentStorage(PersistentStorageContext context)
    {
        _context = context;
    }

    // TODO 
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
            yield return new DbCommitDTO(newestCommit.SHA, newestCommit.AuthorName, newestCommit.Date);
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
        
        if(!Repository.IsValid(dbRepositoryCreate.Filepath))
        {
            return Response.BadRequest;
        }

        DbCommit? newestCommit;
        var realNewestCommit = new Repository(dbRepositoryCreate.Filepath).Commits.FirstOrDefault();
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
            else
            {
                return (Response.Found, repo.NewestCommit);
            }
        }
    }

    public bool IsUpToDate(string filePath, DbCommit? newestCommitDb)
    {
        var repo = new Repository(filePath);
        Commit? newestCommit = repo.Commits.FirstOrDefault();
        if (newestCommit is null && newestCommitDb is null) return true;
        if (newestCommit is null || newestCommitDb is null) return false;
        if (newestCommitDb.SHA == newestCommit.Sha) return true;

        return false;
    }

    public Response Update(string filePath)
    {
        Response response = Delete(filePath);
        if(response == Response.NotFound)
        {
            return response;
        }

        Create(new DbRepositoryCreateDTO(filePath));

        return Response.Updated;
    }

    public Response Delete(string filePath) 
    {
        var repo = _context.Repositories.Find(filePath);
        if (repo is null) return Response.NotFound;

        foreach (var commit in FindAllCommitsFromNewestCommit(repo.NewestCommit))
        {
            _context.Commits
            .Where(c => c.SHA == commit.SHA)
            .ForEachAsync(c => _context.Commits.Remove(c));
        }

        _context.Repositories.Remove(repo);
        _context.SaveChanges();

        return Response.NoContent;
    }
}