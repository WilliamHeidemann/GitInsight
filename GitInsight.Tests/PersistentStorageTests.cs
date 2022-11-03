using LibGit2Sharp;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GitInsight.Tests;

public class PersistentStorageTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly PersistentStorageContext _context; 
    private readonly PersistentStorage _persistentStorage;
    private const string Filepath = "";

    public PersistentStorageTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<PersistentStorageContext>();
        builder.UseSqlite(_connection);
        _context = new PersistentStorageContext(builder.Options);
        _context.Database.EnsureCreated();

        _persistentStorage = new PersistentStorage(_context);
    }
    
    [Fact]
    public void Query_Existing_Repository_Returns_Repository()
    {
        // Arrange
        
        // Act
        var repository = _persistentStorage.GetRepository(Filepath);

        // Assert
        repository.Should().NotBeNull();
    }

    [Fact]
    public void Query_Newest_Commit_From_Repository_Returns_Commit()
    {
        // Arrange
        
        // Act

        // Assert
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}