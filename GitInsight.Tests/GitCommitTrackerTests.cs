using System.Reflection;
using LibGit2Sharp;

namespace GitInsight.Tests;

public class GitCommitTrackerTests
{
    private readonly string _testPath;

    public GitCommitTrackerTests() 
    {
        _testPath = "../../../../GitTestRepo";
    }

    [Fact]
    public async Task GetCommitFrequency_Returns_Correct_Output_for_Test_Repo() 
    {
        //Arrange
        var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = await File.ReadAllLinesAsync("../../../ExpectedCommitFrequencyLog.txt");
        
        //Act
        var actual = gitCommitTracker.GetCommitFrequency();
        
        // Assert
        actual.Should().Equal(expectedOutput);
    }

    [Fact]
    public async Task GetCommitFrequency_Should_Not_Be_Wrong_Output() 
    {
        //Arrange
        var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = await File.ReadAllLinesAsync("../../../WrongOutput.txt");
        
        //Act
        var actual = gitCommitTracker.GetCommitFrequency();
        
        // Assert
        actual.Should().NotEqual(expectedOutput);
    }

    [Fact]
    public async Task GetCommitAuthor_Returns_Correct_Output_for_Test_Repo() 
    {
        //Arrange
        var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = await File.ReadAllLinesAsync("../../../ExpectedCommitAuthorLog.txt");
        
        //Act
        var actual = gitCommitTracker.GetCommitAuthor();
        
        // Assert
        actual.Should().Equal(expectedOutput);
    }    

    [Fact]
    public async Task GetCommitAuthor_Should_Not_Be_Wrong_Output() 
    {
        //Arrange
        var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = await File.ReadAllLinesAsync("../../../WrongOutput.txt");
        
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
}