namespace GitInsight.Tests;

public class DbCommitPersistentStorageTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly PersistentStorageContext _context; 
    private readonly DbCommitPersistentStorage _dbCommitPersistentStorage;

    public DbCommitPersistentStorageTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<PersistentStorageContext>();
        builder.UseSqlite(_connection);
        _context = new PersistentStorageContext(builder.Options);
        _context.Database.EnsureCreated();

        _context.Commits.Add(new DbCommit{
            SHA = "created",
            AuthorName = "SuperDan",
            Date = DateTime.Now,
            RepoId = 0
        });
        _context.Commits.Add(new DbCommit{
            SHA = "created1",
            AuthorName = "SuperDan",
            Date = DateTime.Now,
            RepoId = 0
        });
        _context.Commits.Add(new DbCommit{
            SHA = "created2",
            AuthorName = "SuperDan",
            Date = DateTime.Now,
            RepoId = 0
        });
        _context.Commits.Add(new DbCommit{
            SHA = "created3",
            AuthorName = "SuperDan",
            Date = DateTime.Now,
            RepoId = 2
        });

        _context.SaveChanges();

        _dbCommitPersistentStorage = new DbCommitPersistentStorage(_context);
    }
    
    [Fact]
    public async Task CreateAsync_Returns_Created_And_Sha() 
    {
        // Arrange
        var dbCommitDTO = new DbCommitCreateDTO("test", "Bob", DateTime.Now, 1);
        
        // Act
        var (sha, response) = _dbCommitPersistentStorage.Create(dbCommitDTO);

        // Assert
        sha.Should().Be(dbCommitDTO.SHA);
        response.Should().Be(Response.Created);
    }

    [Fact]
    public async Task Create_Existing_Returns_Conflict_And_Sha() 
    {
        // Arrange
        var dbCommitDTO = new DbCommitCreateDTO("created", "SuperDan", DateTime.Now, 0);
        
        // Act
        var (sha, response) = _dbCommitPersistentStorage.Create(dbCommitDTO);

        // Assert
        sha.Should().Be(dbCommitDTO.SHA);
        response.Should().Be(Response.Conflict);
    }

    [Fact]
    public async Task Delete_Existing_Returns_NoContent() 
    {
        // Arrange
        var deleteSha = "created";
        
        // Act
        var response = await _dbCommitPersistentStorage.DeleteAsync(deleteSha);
        var actual = await _context.Commits.FindAsync(deleteSha, 0);

        // Assert
        response.Should().Be(Response.NoContent);
        actual.Should().BeNull();
    }

    [Fact]
    public async Task Delete_NonExisting_Returns_NotFound() 
    {
        // Arrange
        var deleteSha = "nonExisting";
        
        // Act
        var response = await _dbCommitPersistentStorage.DeleteAsync(deleteSha);

        // Assert
        response.Should().Be(Response.NotFound);
    }

    [InlineData(0, new[] {"created", "created1", "created2"})]
    [InlineData(2, new[] {"created3"})]
    [Theory]
    public void FindAllCommitsByRepoIdAsync_Given_Repo_Returns_Correct_Commits_And_Found(int repoId, IEnumerable<string> commitSHAs) 
    {
        // Arrange
        
        
        // Act
        var (commits, response) =  _dbCommitPersistentStorage.FindAllCommitsByRepoId(repoId);
        var commitsAndExpectedSHAs = commits.Zip(commitSHAs);

        // Assert
        foreach (var item in commitsAndExpectedSHAs)
        {
            item.First.SHA.Should().Be(item.Second);
        }
        response.Should().Be(Response.Found);
    }

    [Fact]
    public void FindAllCommitsByRepoIdAsync_Given_NonExistingRepoId_Returns_No_Commits_And_Found() 
    {
        // Arrange
        var repoId = 5;
        
        // Act
        var (commits, response) = _dbCommitPersistentStorage.FindAllCommitsByRepoId(repoId);

        // Assert
        commits.Count().Should().Be(0);
        response.Should().Be(Response.Found);
    }
    
    [Fact]
    public async Task FindAsync_Given_Sha_Returns_Correct_Commits_And_Found() 
    {
        // Arrange
        var SHA = "created";
        
        // Act
        var (commit, response) = await _dbCommitPersistentStorage.FindAsync(SHA);

        // Assert
        commit!.SHA.Should().Be(SHA);
        response.Should().Be(Response.Found);
    }

    [Fact]
    public async Task FindAsync_Given_NonExisting_Sha_Returns_null_And_NotFound() 
    {
        // Arrange
        var SHA = "nonExisting";
        
        // Act
        var (commit, response) = await _dbCommitPersistentStorage.FindAsync(SHA);

        // Assert
        commit.Should().BeNull();
        response.Should().Be(Response.NotFound);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
} 