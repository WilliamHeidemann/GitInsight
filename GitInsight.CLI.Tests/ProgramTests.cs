using System.Reflection;

namespace GitInsight.CLI.Tests;
public class ProgramTests {

    [Fact]
    public void MainReturnsCorrectOutputForFrequencyModeInTestRepo()
    {
        // Arrange
        using var writer = new StringWriter();
        using var reader = new StringReader("../../../../GitTestRepo");
        Console.SetIn(reader);
        Console.SetOut(writer);

        var outputFile = File.OpenText("../../../files/ExpectedOutput.txt").ReadToEnd().TrimEnd();
        // Act
        var program = Assembly.Load(nameof(GitInsight.CLI));
        program.EntryPoint?.Invoke(null, new[] { Array.Empty<string>() });
        // Assert
        var output = writer.GetStringBuilder().ToString().TrimEnd();
        output.Should().Be(outputFile);
    }
}