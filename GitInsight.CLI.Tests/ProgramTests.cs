using System.Reflection;

namespace GitInsight.CLI.Tests;
public class ProgramTests {
    [Fact]
    public void PrintFrequencyMode_Prints_Grouped_By_Date()
    {
        // Given
        var expectedOutput = new StringWriter();
        expectedOutput.WriteLine("     5 11/09/2022");
        expectedOutput.WriteLine("     4 12/09/2022");
        expectedOutput.WriteLine("     2 14/09/2022");
        expectedOutput.WriteLine("     3 15/09/2022");

        var output = new StringWriter();
        Console.SetOut(output);
        var commits = new List<CommitCountDTO>()
        {
            new(5, new(2022, 9, 11)),
            new(4, new(2022, 9, 12)),
            new(2, new(2022, 9, 14)),
            new(3, new(2022, 9, 15))
        };
    
        // When
        GitInsight.CLI.Program.printCommitCountLines(commits);
    
        // Then
        output.ToString().Should().Be(expectedOutput.ToString());
    }

    [Fact]
    public void AuthorMode_Prints_Grouped_By_Author_And_Date()
    {
        // Given
        var expectedOutput = new StringWriter();
        expectedOutput.WriteLine("Andreas");
        expectedOutput.WriteLine("     5 11/09/2022");
        expectedOutput.WriteLine("     4 12/09/2022");
        expectedOutput.WriteLine();
        expectedOutput.WriteLine("Rakul");
        expectedOutput.WriteLine("     2 14/09/2022");
        expectedOutput.WriteLine("     3 15/09/2022");
        expectedOutput.WriteLine();

        var output = new StringWriter();
        Console.SetOut(output);

        var AndreasCommits = new List<CommitCountDTO>(){
            new(5, new(2022, 9, 11)),
            new(4, new(2022, 9, 12))
        };
        var RakulCommits = new List<CommitCountDTO>(){
            new(2, new(2022, 9, 14)),
            new(3, new(2022, 9, 15))
        };
        var AndreasAuthorCount = new AuthorCommitDTO(AndreasCommits, "Andreas");
        var RakulAuthorCount = new AuthorCommitDTO(RakulCommits, "Rakul");
        
        // When
        GitInsight.CLI.Program.printAuthorLines(new List<AuthorCommitDTO>(){AndreasAuthorCount, RakulAuthorCount});
    
        // Then
        output.ToString().Should().Be(expectedOutput.ToString());
    }
}