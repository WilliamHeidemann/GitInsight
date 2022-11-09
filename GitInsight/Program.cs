// See https://aka.ms/new-console-template for more information
using CommandLine;
using GitInsight;
using System.IO.Compression;

public class Program
{
    class Options 
    {
        [Option('h',"repo-path", Required=true, HelpText = "Insert the path to the repository")]
        public string RepoPath { get; set; } = null!;

        [Option('a', "author-mode", HelpText = "Switch to author-mode.")]
        public bool AuthorMode { get; set; }

    }
    public static async Task Main(string[] args)
    {
        
        var input = Parser.Default.ParseArguments<Options>(args);            
        var gitCommitTracker = new GitCommitTracker();
        
        var factory = new PersistentStorageContextFactory();
        var context = factory.CreateDbContext(Array.Empty<string>());
        var persistentStorageController = new PersistentStorageController(new DbCommitPersistentStorage(context), new DbRepositoryPersistentStorage(context));
        
        var commitsToAnalyze = await persistentStorageController.FindAllCommitsAsync(input.Value.RepoPath);
        
        if (input.Value.AuthorMode)
        {
            Console.WriteLine("-------AUTHOR COMMITS-------");
            gitCommitTracker.GetCommitAuthor(commitsToAnalyze).ToList().ForEach(Console.WriteLine);
        }
        else
        {
            Console.WriteLine("-------COMMIT FREQUENCY-------");
            gitCommitTracker.GetCommitFrequency(commitsToAnalyze).ToList().ForEach(Console.WriteLine);
        }
        
    }
}
