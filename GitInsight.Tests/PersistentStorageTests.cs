using LibGit2Sharp;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using GitInsight.Infrastructure;
using GitInsight.Core;
using System.IO.Compression;

namespace GitInsight.Tests;

public class PersistentStorageTests : IDisposable
{
    
    private readonly SqliteConnection _connection;
    private readonly PersistentStorageContext _context; 
    private readonly PersistentStorage _persistentStorage;
    private readonly string _extractPath;
    private const string NonexistingFilepath = "/newPath"; // Does not exist in database

    public PersistentStorageTests()
    {
        string zipPath = "../../../test-repo/GitTestRepo.zip";
        _extractPath = "../../../test-repo/unzipped/";

        System.IO.Directory.CreateDirectory(_extractPath);
        
        ZipFile.ExtractToDirectory(zipPath, _extractPath);


        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<PersistentStorageContext>();
        builder.UseSqlite(_connection);
        _context = new PersistentStorageContext(builder.Options);
        _context.Database.EnsureCreated();

        _persistentStorage = new PersistentStorage(_context);

        _persistentStorage.Create(new DbRepositoryCreateDTO(_extractPath));
        _context.SaveChanges();
    }
    /*
    [Fact]
    public void Find_Existing_Repository_Without_Commit_Returns_Response_And_Null()
    {
        // Arrange
        
        // Act
        var (response, commit) = _persistentStorage.FindNewestCommit(_extractPath);
        // Assert
        commit.Should().BeNull();
        response.Should().Be(Response.Found);
    }

    [Fact]
    public void Find_Existing_Repository_With_Commit_Returns_Response_And_Commit()
    {
        // Arrange
        
        // Act
        var (response, commit) = _persistentStorage.FindNewestCommit(_extractPath);
        // Assert
        commit.Should().BeNull();
        response.Should().Be(Response.Found);
    }

    [Fact]
    public void Update_Existing_Repository_Returns_Response_Updated()
    {
        // Arrange
        var dbRepo = new DbRepositoryUpdateDTO(_extractPath, "shavalue");
        
        // Act
        var response = _persistentStorage.Update(_extractPath);

        // Assert
        response.Should().Be(Response.Updated);
        
        var (res, commit) = _persistentStorage.FindNewestCommit(_extractPath);
        commit!.SHA.Should().Be("shavalue");
    }

    [Fact]
    public void Update_Nonexisting_Repository_Returns_Response_Notfound()
    {
        // Arrange
        var dbRepo = new DbRepositoryUpdateDTO(NonexistingFilepath, "shavalue");
        
        // Act
        var response = _persistentStorage.Update(NonexistingFilepath);

        // Assert
        response.Should().Be(Response.NotFound);
    }

    [Fact]
    public void Create_Existing_Repository_Returns_Response_Conflict()
    {
        // Arrange
        var dbRepo = new DbRepositoryCreateDTO(_extractPath);
        
        // Act
        var response = _persistentStorage.Create(dbRepo);

        // Assert
        response.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Create_Nonexisting_Repository_Returns_Response_Created()
    {
        // Arrange
        var dbRepo = new DbRepositoryCreateDTO(NonexistingFilepath);
        
        // Act
        Action act = () => _persistentStorage.Create(dbRepo);

        // Assert
        act.Should().Throw<RepositoryNotFoundException>();
    }
    
    */
    public void Dispose()
    {
        System.IO.Directory.Delete(_extractPath, true);

        _context.Dispose();
        _connection.Dispose();
    }
}