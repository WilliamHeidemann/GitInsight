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
    private readonly string _emptyRepoPath;
    private readonly string _singleCommitRepoPath;

    private const string NonexistingFilepath = "/newPath"; // Does not exist in database

    public PersistentStorageTests()
    {
        string zipPath = "../../../test-repo/NewTestRepos.zip";
        _extractPath = "../../../test-repo/new-unzipped/";

        _emptyRepoPath = _extractPath + "Empty-test-repo/.git";
        _singleCommitRepoPath = _extractPath + "Single-commit-repo/.git";

        System.IO.Directory.CreateDirectory(_extractPath);
        
        ZipFile.ExtractToDirectory(zipPath, _extractPath);


        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<PersistentStorageContext>();
        builder.UseSqlite(_connection);
        _context = new PersistentStorageContext(builder.Options);
        _context.Database.EnsureCreated();

        _persistentStorage = new PersistentStorage(_context);

        _persistentStorage.Create(new DbRepositoryCreateDTO(_emptyRepoPath));

        _context.SaveChanges();
    }
    
    [Fact]
    public void Find_Existing_Repository_Without_Commit_Returns_Response_And_Null()
    {
        // Arrange
        
        // Act
        var (response, commit) = _persistentStorage.FindNewestCommit(_emptyRepoPath);

        // Assert
        commit.Should().BeNull();
        response.Should().Be(Response.Found);
    }
    

    [Fact]
    public void FindNewestCommit_From_Existing_Repository_With_Single_Commit_Returns_Response_And_Commit()
    {
        // Arrange
        var realCommit = new Repository(_singleCommitRepoPath).Commits.FirstOrDefault()!;
        _persistentStorage.Create(new DbRepositoryCreateDTO(_singleCommitRepoPath));
        // Act
        var (response, commit) = _persistentStorage.FindNewestCommit(_singleCommitRepoPath);

        // Assert
        commit!.SHA.Should().Be(realCommit.Sha);
        response.Should().Be(Response.Found);
    }
    /*
    [Fact]
    public void Update_Existing_Repository_Returns_Response_Updated()
    {
        // Arrange
        var dbRepo = new DbRepositoryUpdateDTO(_singleCommitRepoPath);
        _persistentStorage.Create(new DbRepositoryCreateDTO(_singleCommitRepoPath));

        var realRepo = new Repository(_singleCommitRepoPath);

        // Act
        var response = _persistentStorage.Update(dbRepo.Filepath);
        var (res, commit) = _persistentStorage.FindNewestCommit(dbRepo.Filepath);

        // Assert
        response.Should().Be(Response.Updated);
        
        commit!.SHA.Should().Be(realRepo.Commits.FirstOrDefault()!.Sha);
    }
    */
    [Fact]
    public void Update_Nonexisting_Repository_Returns_Response_Notfound()
    {
        // Arrange
        var dbRepo = new DbRepositoryUpdateDTO(NonexistingFilepath);
        
        // Act
        var response = _persistentStorage.Update(dbRepo.Filepath);

        // Assert
        response.Should().Be(Response.NotFound);
    }

    [Fact]
    public void Create_Existing_Repository_Returns_Response_Conflict()
    {
        // Arrange
        var dbRepo = new DbRepositoryCreateDTO(_extractPath);
        
        // Act        
        var response = _persistentStorage.Create(new DbRepositoryCreateDTO(_emptyRepoPath));

        // Assert
        response.Should().Be(Response.Conflict);
    }
    

    [Fact]
    public void Create_valid_Repository_Returns_Created() 
    {
         // Arrange
        var dbRepo = new DbRepositoryCreateDTO(_singleCommitRepoPath);
        
        // Act
        var response = _persistentStorage.Create(dbRepo);

        // Assert
        response.Should().Be(Response.Created);
    }

    [Fact]
    public void Create_Nonexisting_Repository_Returns_Bad_Request()
    {
        // Arrange
        var dbRepo = new DbRepositoryCreateDTO(NonexistingFilepath);
        
        // Act
        Response actual = _persistentStorage.Create(dbRepo);

        // Assert
        actual.Should().Be(Response.BadRequest);
    }

    public void Dispose()
    {
        System.IO.Directory.Delete(_extractPath, true);

        _context.Dispose();
        _connection.Dispose();
    }
}