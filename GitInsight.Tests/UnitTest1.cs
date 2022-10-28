using System.Reflection;
using LibGit2Sharp;

namespace GitInsight.Tests;

public class UnitTest1
{
    [Fact]
    public void Running_Program_Outputs_Expected_Log()
    {
        // Arrange
        using var writer = new StringWriter();
        Console.SetOut(writer);
        var expectedOutput = File.OpenText("../../../ExpectedOutput.txt").ReadToEnd().TrimEnd();

        // Act
        var program = Assembly.Load(nameof(GitCommitTracker));
        
        program.EntryPoint?.Invoke(null, new[] { Array.Empty<string>() });

        // Assert
        var output = writer.GetStringBuilder().ToString().TrimEnd();
        output.Should().Be(expectedOutput);
    }
    
    
    // [Fact]
    // public void Running_Program_With_Frequency_Flag_Outputs_Frequency_Log()
    // {
    //     // Arrange
    //     using var writer = new StringWriter();
    //     Console.SetOut(writer);
    //     var expectedOutput = File.OpenText("../../../ExpectedCommitFrequencyLog.txt").ReadToEnd().TrimEnd();
    //
    //     // Act
    //     var program = Assembly.Load(nameof(GitInsight));
    //     program.EntryPoint?.Invoke(null, new[] { Array.Empty<string>() });
    //
    //     // Assert
    //     var output = writer.GetStringBuilder().ToString().TrimEnd();
    //     output.Should().Be(expectedOutput);
    // }
    //
    // [Fact]
    // public void Running_Program_With_Author_Flag_Outputs_Author_Log()
    // {
    //     // Arrange
    //     using var writer = new StringWriter();
    //     Console.SetOut(writer);
    //     var expectedOutput = File.OpenText("../../../ExpectedCommitFrequencyLog.txt").ReadToEnd().TrimEnd();
    //
    //     // Act
    //     var program = Assembly.Load(nameof(GitInsight));
    //     
    //     program.EntryPoint?.Invoke(null, new[] { Array.Empty<string>() });
    //
    //     // Assert
    //     var output = writer.GetStringBuilder().ToString().TrimEnd();
    //     output.Should().Be(expectedOutput);
    // }
}