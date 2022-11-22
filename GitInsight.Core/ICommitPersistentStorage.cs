namespace GitInsight.Core;

public interface ICommitPersistentStorage{
    Results<Created<DbCommitCreateDTO>, Conflict<string>> Create(DbCommitCreateDTO dbCommitCreate);
    Task<Results<Ok<DbCommitDTO>, NotFound<string>>> FindAsync(string SHA);
    Task<Results<NoContent, NotFound<string>>> DeleteAsync(string SHA);
    Results<Ok<IReadOnlyCollection<DbCommitDTO>>, NotFound<int>> FindAllCommitsByRepoId(int repoId);
}