namespace GitInsight.Tests;

public class PersistentStorageControllerTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly PersistentStorageContext _context; 
    private readonly PersistentStorageController _persistentStorageController;
    private const string ExtractPath = "../../../test-repo/PersistantStorageControllerTests-unzipped/";
    private const string EmptyRepoPath = ExtractPath + "Empty-test-repo/.git";
    private const string SingleCommitRepoPath = ExtractPath + "Single-commit-repo/.git";
    private const string TwoCommitRepoPath = ExtractPath + "Two-commit-repo/.git";
    private const string ThreeCommitRepoPath = ExtractPath + "Multiple-commit-repo/.git";
    private const string NonexistingFilepath = "/newPath"; // Does not exist in database

    public PersistentStorageControllerTests()
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

        var dbCommitPersistentStorage = new DbCommitPersistentStorage(_context);
        var dbRepositoryPersistentStorage = new DbRepositoryPersistentStorage(_context);

        _persistentStorageController = new PersistentStorageController(dbCommitPersistentStorage, dbRepositoryPersistentStorage);
    }
    
    [InlineData(EmptyRepoPath)]
    [InlineData(SingleCommitRepoPath)]
    [InlineData(TwoCommitRepoPath)]
    [InlineData(ThreeCommitRepoPath)]
    [Theory]
    public async Task FindAllCommitsAsync_Creates_Repo_Given_New_Repo(string filePath) 
    {
        // Arrange

        // Act
        var actual = await _persistentStorageController.FindAllCommitsAsync(filePath);
        var repo = _context.Repositories.FirstOrDefault(r => r.FilePath == filePath);

        // Assert
        repo.Should().NotBeNull();
    }

    [Fact]
    public async Task FindAllCommitsAsync_Throws_Exception_Given_NonexistentPath()
    {
        // Act
        Func<Task> act = () => _persistentStorageController.FindAllCommitsAsync(NonexistingFilepath);

        // Assert
        await act.Should().ThrowAsync<RepositoryNotFoundException>();

    }

    [InlineData(EmptyRepoPath)]
    [Theory]
    public async Task FindAllCommitsAsync_Given_New_Repo(string filePath)
    {
        // Arrange

        // Act
        var actual = await _persistentStorageController.FindAllCommitsAsync(filePath);

        var repoId = _context.Repositories.FirstOrDefault(r => r.FilePath == filePath)!.Id;
        var expected = new List<DbCommitDTO>();
        await _context.Commits.Where(c => c.RepoId == repoId).ForEachAsync(c => {
            expected.Add(new DbCommitDTO(c.SHA, c.AuthorName, c.Date));
        });

        // Assert
        actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    public void Dispose()
    {
        System.IO.Directory.Delete(ExtractPath, true);

        _context.Dispose();
        _connection.Dispose();
    }
}