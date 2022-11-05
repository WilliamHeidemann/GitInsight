using System.Collections;
using System.Reflection;
using LibGit2Sharp;
using System.IO.Compression;
using GitInsight.Core;
using Moq;

namespace GitInsight.Tests;

public class GitCommitTrackerTests
{
    [Fact]
    public void Commit_Frequency_With_CommitDTO_Of_DateTimeNow_William_Returns_Correct_Output()
    {
        //Arrange
        var commitTracker = new GitCommitTracker();
        var commit1 = new DbCommitDTO("123", "William", DateTime.Now, "456");
        var commit2 = new DbCommitDTO("321", "Rakul", DateTime.Now, "654");
        var commit3 = new DbCommitDTO("132", "Andreas", DateTime.Now.AddDays(1), "645");
        var commitsToAnalyze = new List<DbCommitDTO> { commit1, commit2, commit3 };

        //Act
        var result = commitTracker.GetCommitFrequency(commitsToAnalyze);
        var expectedOutput = File.ReadAllLines("../../../files/ShortFrequencyCommitLog.txt");

        //Assert
        result.Should().BeEquivalentTo(expectedOutput);
    }
    
    [Fact]
    public void Commit_Author_With_CommitDTO_Of_DateTimeNow_William_Returns_Correct_Output()
    {
        //Arrange
        var commitTracker = new GitCommitTracker();
        var commit1 = new DbCommitDTO("123", "William", DateTime.Now, "456");
        var commit2 = new DbCommitDTO("321", "Rakul", DateTime.Now, "654");
        var commit3 = new DbCommitDTO("132", "Andreas", DateTime.Now.AddDays(1), "645");
        var commitsToAnalyze = new List<DbCommitDTO> { commit1, commit2, commit3 };

        //Act
        var result = commitTracker.GetCommitAuthor(commitsToAnalyze);
        var expectedOutput = File.ReadAllLines("../../../files/ShortAuthorCommitLog.txt");
        
        //Assert
        result.Should().BeEquivalentTo(expectedOutput);
    }
}