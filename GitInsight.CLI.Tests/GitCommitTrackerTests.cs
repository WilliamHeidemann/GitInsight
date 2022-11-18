namespace GitInsight.CLI.Tests;

public class GitCommitTrackerTests
{

    [Fact]
    public void Commit_Frequency_Returns_Expected_Output()
    {
        //Arrange
        var commitTracker = new GitCommitTracker();
        var commit1 = new DbCommitDTO("sha", "William", new DateTime(2022, 11, 5));
        var commit2 = new DbCommitDTO("sha","Rakul", new DateTime(2022, 11, 5));
        var commit3 = new DbCommitDTO("sha","Andreas", new DateTime(2022, 11, 6));
        var commitsToAnalyze = new List<DbCommitDTO> { commit1, commit2, commit3 };

        //Act
        var result = commitTracker.GetCommitFrequency(commitsToAnalyze);
        var expectedOutput = File.ReadAllLines("../../../files/ShortFrequencyCommitLog.txt");

        //Assert
        result.Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public void Commit_Frequency_Should_Not_Be_Unexpected_Output()
    {
        //Arrange
        var commitTracker = new GitCommitTracker();
        var commit1 = new DbCommitDTO("sha", "William", new DateTime(2022, 11, 5));
        var commit2 = new DbCommitDTO("sha", "Rakul", new DateTime(2022, 11, 5));
        var commit3 = new DbCommitDTO("sha", "Andreas", new DateTime(2022, 11, 6));
        var commitsToAnalyze = new List<DbCommitDTO> { commit1, commit2, commit3 };

        //Act
        var result = commitTracker.GetCommitFrequency(commitsToAnalyze);
        var expectedOutput = File.ReadAllLines("../../../files/WrongOutput.txt");

        //Assert
        result.Should().NotBeEquivalentTo(expectedOutput);
    }

    [Fact]
    public void Commit_Frequency_With_Empty_List_Should_Be_Empty()
    {
        //Arrange
        var commitTracker = new GitCommitTracker();
        var commitsToAnalyze = new List<DbCommitDTO>();
        //Act
        var result = commitTracker.GetCommitFrequency(commitsToAnalyze);
        
        //Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void Commit_Author_Returns_Expected_Output()
    {
        //Arrange
        var commitTracker = new GitCommitTracker();
        var commit1 = new DbCommitDTO("sha", "William", new DateTime(2022, 11, 5));
        var commit2 = new DbCommitDTO("sha", "Rakul", new DateTime(2022, 11, 5));
        var commit3 = new DbCommitDTO("sha", "Andreas", new DateTime(2022, 11, 6));
        var commitsToAnalyze = new List<DbCommitDTO> { commit1, commit2, commit3 };
        //Act
        var result = commitTracker.GetCommitAuthor(commitsToAnalyze);
        var expectedOutput = File.ReadAllLines("../../../files/ShortAuthorCommitLog.txt");
        
        //Assert
        result.Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public void Commit_Author_Should_Not_Be_Unexpected_Output()
    {
        //Arrange
        var commitTracker = new GitCommitTracker();
        var commit1 = new DbCommitDTO("sha", "William", new DateTime(2022, 11, 5));
        var commit2 = new DbCommitDTO("sha", "Rakul", new DateTime(2022, 11, 5));
        var commit3 = new DbCommitDTO("sha", "Andreas", new DateTime(2022, 11, 6));
        var commitsToAnalyze = new List<DbCommitDTO> { commit1, commit2, commit3 };
        //Act
        var result = commitTracker.GetCommitAuthor(commitsToAnalyze);
        var expectedOutput = File.ReadAllLines("../../../files/WrongOutput.txt");
        
        //Assert
        result.Should().NotBeEquivalentTo(expectedOutput);
    }

    [Fact]
    public void Commit_Author_With_Empty_List_Should_Be_Empty()
    {
        //Arrange
        var commitTracker = new GitCommitTracker();
        var commitsToAnalyze = new List<DbCommitDTO>();
        //Act
        var result = commitTracker.GetCommitAuthor(commitsToAnalyze);
        
        //Assert
        result.Should().BeEmpty();
    }

}
