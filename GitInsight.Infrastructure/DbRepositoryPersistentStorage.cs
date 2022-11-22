using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GitInsight.Infrastructure.Tests")]
namespace GitInsight.Infrastructure;
internal class DbRepositoryPersistentStorage : IRepositoryPersistentStorage
{
    private PersistentStorageContext _context;
    private DbRepositoryValidator _validator;
    public DbRepositoryPersistentStorage(PersistentStorageContext context, DbRepositoryValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Results<Created<DbRepositoryCreateDTO>, ValidationProblem, Conflict<int>>> CreateAsync(DbRepositoryCreateDTO dbRepositoryCreate)
    {
        var validation = await _validator.ValidateAsync(dbRepositoryCreate);

        if (!validation.IsValid) return TypedResults.ValidationProblem(validation.ToDictionary());

        var entity = await _context.Repositories.FirstOrDefaultAsync(t => t.FilePath == dbRepositoryCreate.Filepath);
        if(entity is not null) return TypedResults.Conflict(entity.Id);
       
        entity = new DbRepository(dbRepositoryCreate.Filepath)
        {
            NewestCommitSHA = null // should be set later
        };

        _context.Repositories.Add(entity);
        await _context.SaveChangesAsync();

        return TypedResults.Created($"{entity.FilePath}", dbRepositoryCreate with { Filepath = entity.FilePath });
    }

    public async Task<Results<Ok<DbRepositoryDTO>, NotFound<string>>> FindAsync(string filePath)
    {
        var repo = await _context.Repositories.FirstOrDefaultAsync(t => t.FilePath == filePath);
        if(repo is null) return TypedResults.NotFound(filePath);
        return TypedResults.Ok(new DbRepositoryDTO(repo.Id, repo.FilePath, repo.NewestCommitSHA!));
    }

    public async Task<Results<NoContent, NotFound<int>>> UpdateNewestCommitSHA(string SHA, int repoId)
    {
        var repo = await _context.Repositories.FirstOrDefaultAsync(t => t.Id == repoId);
        if(repo is null) return TypedResults.NotFound(repoId);
        repo!.NewestCommitSHA = SHA;
        await _context.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}
