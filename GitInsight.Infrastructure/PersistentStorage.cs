namespace GitInsight.Infrastructure;

public class PersistentStorage : IPersistentStorage
{
    private PersistentStorageContext _context;
    public PersistentStorage(PersistentStorageContext context)
    {
        _context = context;
    }

    public Response Create(DbRepositoryCreateDTO dbRepositoryCreate)
    {
        var repo = _context.Repositories.FirstOrDefault(t => t.Filepath == dbRepositoryCreate.Filepath);

        if (repo is not null) {
            return Response.Conflict;
        }

        var newRepo = new DbRepository{
            Filepath = dbRepositoryCreate.Filepath
        };

        _context.Add(newRepo);
        return Response.Created;
    }

    public (Response, DbCommitDTO?) Find(string Filepath)
    {
        var repo = _context.Repositories.Find(Filepath);

        if (repo is null) {
            return (Response.NotFound, null);
        } else {
            var commit = _context.Commits.Find(repo.NewestCommit);
            if (commit is null) {
                return (Response.Found, null);
            } else {
                return (Response.Found, new DbCommitDTO(commit!.SHA, commit.AuthorName, commit.Date, commit.ParentCommit.SHA));
            }
        }

        throw new NotImplementedException();
    }

    public Response Update(DbRepositoryUpdateDTO dbRepositoryUpdate)
    {
        throw new NotImplementedException();
    }
}