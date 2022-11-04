using System.Reflection;
using LibGit2Sharp;
using System.IO.Compression;

namespace GitInsight.Tests;

public class GitCommitTrackerTests : IDisposable
{
    private readonly string _testPath;
    private readonly string _extractPath;

    public GitCommitTrackerTests() 
    {
        _testPath = "../../../test-repo/unzipped/GitTestRepo";
        string zipPath = "../../../test-repo/GitTestRepo.zip";
        _extractPath = "../../../test-repo/unzipped/";

        System.IO.Directory.CreateDirectory(_extractPath);
        
        ZipFile.ExtractToDirectory(zipPath, _extractPath);
    }

    [Fact]
    public void GetCommitFrequency_Returns_Correct_Output_for_Test_Repo() 
    {
        //Arrange
        using var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = File.ReadAllLines("../../../files/ExpectedCommitFrequencyLog.txt");
        
        //Act
        var actual = gitCommitTracker.GetCommitFrequency();
        
        // Assert
        actual.Should().Equal(expectedOutput);
    }
    
    [Fact]
    public async Task GetCommitFrequency_Should_Not_Be_Wrong_Output() 
    {
        //Arrange
        using var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = await File.ReadAllLinesAsync("../../../files/WrongOutput.txt");
        
        //Act
        var actual = gitCommitTracker.GetCommitFrequency();
        
        // Assert
        actual.Should().NotEqual(expectedOutput);
    }

    [Fact]
    public async Task GetCommitAuthor_Returns_Correct_Output_for_Test_Repo() 
    {
        //Arrange
        using var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = await File.ReadAllLinesAsync("../../../files/ExpectedCommitAuthorLog.txt");
        
        //Act
        var actual = gitCommitTracker.GetCommitAuthor();
        
        // Assert
        actual.Should().Equal(expectedOutput);
    }    

    [Fact]
    public async Task GetCommitAuthor_Should_Not_Be_Wrong_Output() 
    {
        //Arrange
        using var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = await File.ReadAllLinesAsync("../../../files/WrongOutput.txt");
        
        //Act
        var actual = gitCommitTracker.GetCommitAuthor();
        
        // Assert
        actual.Should().NotEqual(expectedOutput);
    }  

    [Fact]
    public void Running_Program_With_Invalid_Repo_Should_Give_ArgumentException()
    {
        Action invalidRepo = () => new GitCommitTracker("invalid");

        invalidRepo.Should().Throw<ArgumentException>().WithMessage($"Repository was not found at invalid.");
    }

    public void Dispose()
    {
        System.IO.Directory.Delete(_extractPath, true);
    }
}