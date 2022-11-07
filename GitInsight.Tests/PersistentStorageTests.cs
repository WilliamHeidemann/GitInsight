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
    private const string ExtractPath = "../../../test-repo/new-unzipped/";
    private const string EmptyRepoPath = ExtractPath + "Empty-test-repo/.git";
    private const string SingleCommitRepoPath = ExtractPath + "Single-commit-repo/.git";
    private const string TwoCommitRepoPath = ExtractPath + "Two-commit-repo/.git";
    private const string ThreeCommitRepoPath = ExtractPath + "Multiple-commit-repo/.git";

    private const string NonexistingFilepath = "/newPath"; // Does not exist in database

    public PersistentStorageTests()
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

        _persistentStorage = new PersistentStorage(_context);

        _persistentStorage.Create(new DbRepositoryCreateDTO(EmptyRepoPath));

        _context.SaveChanges();
    }
    
    [Fact]
    public void Find_Existing_Repository_Without_Commit_Returns_Response_And_Null()
    {
        // Arrange
        
        // Act
        var (response, commit) = _persistentStorage.FindNewestCommit(EmptyRepoPath);

        // Assert
        commit.Should().BeNull();
        response.Should().Be(Response.Found);
    }

    [InlineData(SingleCommitRepoPath)]
    [InlineData(TwoCommitRepoPath)]
    [InlineData(ThreeCommitRepoPath)]
    [Theory]
    public void FindNewestCommit_From_Existing_Repository_Returns_Response_And_NewestCommit(string repoPath)
    {
        // Arrange
        var realCommit = new Repository(repoPath).Commits.FirstOrDefault()!;
        _persistentStorage.Create(new DbRepositoryCreateDTO(repoPath));
        // Act
        var (response, commit) = _persistentStorage.FindNewestCommit(repoPath);

        // Assert
        commit!.SHA.Should().Be(realCommit.Sha);
        response.Should().Be(Response.Found);
    }

    [InlineData(SingleCommitRepoPath)]
    [InlineData(TwoCommitRepoPath)]
    [InlineData(ThreeCommitRepoPath)]
    [Theory]
    public void FindAllCommitsFromNewestCommit_Returns_Commits_In_Right_Order(string repoPath) 
    {
        // Arrange
        var realCommits = new Repository(repoPath).Commits.AsEnumerable();
        _persistentStorage.Create(new DbRepositoryCreateDTO(repoPath));
        var (response, newCommit) = _persistentStorage.FindNewestCommit(repoPath);

        // Act
        var commits = _persistentStorage.FindAllCommitsFromNewestCommit(newCommit);
        var realAndDbCommits = commits.Zip(realCommits);

        // Assert
        foreach(var item in realAndDbCommits) 
        {
            item.First.SHA.Should().Be(item.Second.Sha);
        }
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

    [InlineData(SingleCommitRepoPath)]
    [InlineData(TwoCommitRepoPath)]
    [InlineData(ThreeCommitRepoPath)]
    [Theory]
    public void Create_Returns_Created_For_Valid_Repos(string repoPath)
    {
        // Arrange
        var dbRepo = new DbRepositoryCreateDTO(repoPath);
        
        // Act        
        var response = _persistentStorage.Create(dbRepo);

        // Assert
        response.Should().Be(Response.Created);
    }

    [Fact]
    public void Create_Existing_Repository_Returns_Response_Conflict()
    {
        // Arrange
        var dbRepo = new DbRepositoryCreateDTO(EmptyRepoPath);
        
        // Act        
        var response = _persistentStorage.Create(dbRepo);

        // Assert
        response.Should().Be(Response.Conflict);
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
        System.IO.Directory.Delete(ExtractPath, true);

        _context.Dispose();
        _connection.Dispose();
    }
}