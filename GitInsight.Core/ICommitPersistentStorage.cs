namespace GitInsight.Core;

public interface ICommitPersistentStorage{
    (string, Response) Create(DbCommitCreateDTO dbCommitCreate);
    Task<(DbCommitDTO?, Response)> FindAsync(string SHA);
    Task<Response> DeleteAsync(string SHA);
    (IReadOnlyCollection<DbCommitDTO>, Response) FindAllCommitsByRepoId(int repoId);
}