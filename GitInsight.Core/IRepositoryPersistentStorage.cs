namespace GitInsight.Core;

public interface IRepositoryPersistentStorage{
    Task<Results<Created<DbRepositoryCreateDTO>, ValidationProblem, Conflict<int>>> CreateAsync(DbRepositoryCreateDTO dbRepositoryCreate);
    Task<Results<Ok<DbRepositoryDTO>, NotFound<string>>> FindAsync(string filePath);
    Task<Results<NoContent, NotFound<int>>> UpdateNewestCommitSHA(string SHA, int repoId);
    
}