using System.Reflection;

namespace GitInsight.Tests;
public class ProgramTests {
    /*
    [Fact]
    public async Task MainReturnsCorrectOutputForFrequencyModeInTestRepo() 
    {
        // Arrange
        using var writer = new StringWriter();
        using var reader = new StringReader("../../../../GitTestRepo");
        Console.SetIn(reader);
        Console.SetOut(writer);

        var outputFile = File.OpenText("../../../ExpectedOutput.txt").ReadToEnd().TrimEnd();
         // Act
        var program = Assembly.Load(nameof(GitInsight));
        program.EntryPoint?.Invoke(null, new[] { Array.Empty<string>() });
        // Assert
        var output = writer.GetStringBuilder().ToString().TrimEnd();
        output.Should().Be(outputFile);
    }
    */
}