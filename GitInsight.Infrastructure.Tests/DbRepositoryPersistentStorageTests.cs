
namespace GitInsight.Infrastructure.Tests;

public class DbRepositoryPersistentStorageTests : IDisposable
{
    
    private readonly SqliteConnection _connection;
    private readonly PersistentStorageContext _context; 
    private readonly DbRepositoryPersistentStorage _dbRepositoryPersistentStorage;
    private const string ExtractPath = "../../../test-repo/new-unzipped/";
    private const string EmptyRepoPath = ExtractPath + "Empty-test-repo/.git";
    private const string SingleCommitRepoPath = ExtractPath + "Single-commit-repo/.git";
    private const string TwoCommitRepoPath = ExtractPath + "Two-commit-repo/.git";
    private const string ThreeCommitRepoPath = ExtractPath + "Multiple-commit-repo/.git";

    private const string NonexistingFilepath = "/newPath"; // Does not exist in database

    public DbRepositoryPersistentStorageTests()
    {
        string zipPath = "../../../test-repo/NewTestRepos.zip";

        System.IO.Directory.CreateDirectory(ExtractPath);
        ZipFile.ExtractToDirectory(zipPath, ExtractPath);

        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<PersistentStorageContext>();
        builder.UseSqlite(_connection);
        _context = new PersistentStorageContext(builder.Options);
        _context.Database.EnsureCreated();

        _dbRepositoryPersistentStorage = new DbRepositoryPersistentStorage(_context);

        _context.Repositories.Add(new Infrastructure.DbRepository(EmptyRepoPath));
        _context.SaveChanges();
    }

    [InlineData(SingleCommitRepoPath, 2)]
    [InlineData(TwoCommitRepoPath, 2)]
    [InlineData(ThreeCommitRepoPath, 2)]
    [Theory]
    public async Task Create_Returns_Created_For_Valid_Repos(string repoPath, int expectedId)
    {
        // Arrange
        var dbRepo = new DbRepositoryCreateDTO(repoPath);
        
        // Act        
        var (id, response) = await _dbRepositoryPersistentStorage.CreateAsync(dbRepo);
        var expected = _context.Repositories.FirstOrDefault(r => r.FilePath == repoPath);

        // Assert
        response.Should().Be(Response.Created);
        id.Should().Be(expectedId);
    }

    [Fact]
    public async Task CreateAsync_Existing_Repository_Returns_Response_Conflict()
    {
        // Arrange
        var dbRepo = new DbRepositoryCreateDTO(EmptyRepoPath);
        
        // Act        
        var (id, response) = await _dbRepositoryPersistentStorage.CreateAsync(dbRepo);

        // Assert
        response.Should().Be(Response.Conflict);
        id.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_Nonexisting_Repository_Returns_Bad_Request()
    {
        // Arrange
        var dbRepo = new DbRepositoryCreateDTO(NonexistingFilepath);
        
        // Act
        var (id, response) = await _dbRepositoryPersistentStorage.CreateAsync(dbRepo);

        // Assert
        response.Should().Be(Response.BadRequest);
        id.Should().Be(-1);
    }

    [Fact]
    public async Task Find_Existing_Empty_Repository_Returns_DTO_And_Found()
    {
        // Arrange
        
        // Act
        var (dto, response) = await _dbRepositoryPersistentStorage.FindAsync(EmptyRepoPath);

        // Assert
        response.Should().Be(Response.Found);
        dto!.Filepath.Should().Be(EmptyRepoPath);
    }

    [Fact]
    public async Task Find_NonExisting_Repository_Returns_null_And_NotFound()
    {
        // Arrange
        
        // Act
        var (dto, response) = await _dbRepositoryPersistentStorage.FindAsync(NonexistingFilepath);

        // Assert
        response.Should().Be(Response.NotFound);
        dto.Should().BeNull();
    }

    [InlineData(SingleCommitRepoPath)]
    [InlineData(TwoCommitRepoPath)]
    [InlineData(ThreeCommitRepoPath)]
    [Theory]
    public async Task Updating_NewestSHA_updates_newest_sha_in_DB(string repoPath)
    {
        // Arrange
        _context.Repositories.Add(new Infrastructure.DbRepository(repoPath));
        _context.SaveChanges();
        var repo = _context.Repositories.FirstOrDefault(r => r.FilePath == repoPath);
        var realRepo = new LibGit2Sharp.Repository(repoPath);
        var realNewestCommitSHA = realRepo.Commits.FirstOrDefault()!.Sha;

    
        // Act
        await _dbRepositoryPersistentStorage.UpdateNewestCommitSHA(realNewestCommitSHA,repo!.Id);
    
        // Assert
        repo.NewestCommitSHA.Should().Be(realNewestCommitSHA);
    }   

    public void Dispose()
    {
        System.IO.Directory.Delete(ExtractPath, true);

        _context.Dispose();
        _connection.Dispose();
    }
}