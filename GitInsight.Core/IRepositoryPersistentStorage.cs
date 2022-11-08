namespace GitInsight.Core;

public interface IRepositoryPersistentStorage{
    Task<(int, Response)> CreateAsync(DbRepositoryCreateDTO dbRepositoryCreate);
    Task<(DbRepositoryDTO?, Response)> FindAsync(string filePath);
    Task<Response> UpdateAsync(DbRepositoryUpdateDTO dbRepositoryUpdateDTO);
    
}