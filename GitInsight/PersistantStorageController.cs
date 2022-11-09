using GitInsight.Core;

namespace GitInsight;

public class PersistentStorageController 
{
    private readonly ICommitPersistentStorage _dbCommitPersistentStorage;
    private readonly IRepositoryPersistentStorage _dbRepositoryPersistentStorage;

    public PersistentStorageController(DbCommitPersistentStorage dbCommitPersistentStorage, DbRepositoryPersistentStorage dbRepositoryPersistentStorage)
    {
        _dbCommitPersistentStorage = dbCommitPersistentStorage;
        _dbRepositoryPersistentStorage = dbRepositoryPersistentStorage;
    }

    public async Task<IEnumerable<DbCommitDTO>> FindAllCommitsAsync(string filePath)
    {     
        if(!Repository.IsValid(filePath)) throw new RepositoryNotFoundException("The Repository does not exist. Please provide a valid filepath.");
        var (id, response) = await _dbRepositoryPersistentStorage.CreateAsync(new DbRepositoryCreateDTO(filePath));
        if(response == Response.Conflict) await _dbRepositoryPersistentStorage.UpdateAsync(new DbRepositoryUpdateDTO(filePath));
        var (commits, findResponse) = await _dbCommitPersistentStorage.FindAllCommitsByRepoIdAsync(id);
        return commits; 
    }
}