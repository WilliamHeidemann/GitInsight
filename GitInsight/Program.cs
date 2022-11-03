// See https://aka.ms/new-console-template for more information
using CommandLine;
using GitInsight;

public class Program
{
    class Options 
    {
        [Option('h',"repo-path", Required=true, HelpText = "Insert the path to the repository")]
        public string RepoPath { get; set; } = null!;

        [Option('a', "author-mode", HelpText = "Switch to author-mode.")]
        public bool AuthorMode { get; set; }

    }
    public static void Main(string[] args)
    {
        var result = Parser.Default.ParseArguments<Options>(args);            
        var gitCommitTracker = new GitCommitTracker(result.Value.RepoPath);
        
        if (result.Value.AuthorMode)
        {
            Console.WriteLine("-------AUTHOR COMMITS-------");
            gitCommitTracker.GetCommitAuthor().ToList().ForEach(Console.WriteLine);
        }
        else
        {
            Console.WriteLine("-------COMMIT FREQUENCY-------");
            gitCommitTracker.GetCommitFrequency().ToList().ForEach(Console.WriteLine);
        }
    }
}
