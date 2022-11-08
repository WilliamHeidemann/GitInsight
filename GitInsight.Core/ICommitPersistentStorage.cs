namespace GitInsight.Core;

public interface ICommitPersistentStorage{
    Task<(string, Response)> CreateAsync(DbCommitCreateDTO dbCommitCreate);
    Task<(DbCommitDTO?, Response)> FindAsync(string SHA);
    Task<Response> DeleteAsync(string SHA);
    Task<(IReadOnlyCollection<DbCommitDTO>, Response)> FindAllCommitsByRepoIdAsync(int repoId);
}