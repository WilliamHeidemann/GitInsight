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
    public void GetCommitFrequency_Returns_Correct_Output_for_Test_Repo() 
    {
        //Arrange
        var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = File.ReadAllLines("../../../ExpectedCommitFrequencyLog.txt");
        
        //Act
        var actual = gitCommitTracker.GetCommitFrequency();
        
        // Assert
        actual.Should().Equal(expectedOutput);
    }

    [Fact]
    public void GetCommitFrequency_Should_Not_Be_Wrong_Output() 
    {
        //Arrange
        var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = File.ReadAllLines("../../../WrongOutput.txt");
        
        //Act
        var actual = gitCommitTracker.GetCommitFrequency();
        
        // Assert
        actual.Should().NotEqual(expectedOutput);
    }

    [Fact]
    public void GetCommitAuthor_Returns_Correct_Output_for_Test_Repo() 
    {
        //Arrange
        var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = File.ReadAllLines("../../../ExpectedCommitAuthorLog.txt");
        
        //Act
        var actual = gitCommitTracker.GetCommitAuthor();
        
        // Assert
        actual.Should().Equal(expectedOutput);
    }    

    [Fact]
    public void GetCommitAuthor_Should_Not_Be_Wrong_Output() 
    {
        //Arrange
        var gitCommitTracker = new GitCommitTracker(_testPath);
        var expectedOutput = File.ReadAllLines("../../../WrongOutput.txt");
        
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