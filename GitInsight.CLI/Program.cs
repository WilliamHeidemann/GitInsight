namespace GitInsight.CLI;
// See https://aka.ms/new-console-template for more information
using CommandLine;
using GitInsight;
using GitInsight.Core;
using System.Globalization;
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
        
        await using var context = new PersistentStorageContext(); 
        await context.Database.MigrateAsync();

        var persistentStorageController = new PersistentStorageController(context);
         
        if (input.Value.AuthorMode)
        {
            try {
                var authorCommits = await persistentStorageController.GetAuthorMode(input.Value.RepoPath);
                Console.WriteLine("-------AUTHOR COMMITS-------");
                printAuthorLines(authorCommits);
            }
            catch (RepositoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            
            // gitCommitTracker.GetCommitAuthor(commitsToAnalyze).ToList().ForEach(Console.WriteLine);
        }
        else
        {
            try {
                var commitCounts = await persistentStorageController.GetFrequencyMode(input.Value.RepoPath);
                Console.WriteLine("-------COMMIT FREQUENCY-------");
                printCommitCountLines(commitCounts);            
            }
            catch (RepositoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            // gitCommitTracker.GetCommitFrequency(commitsToAnalyze).ToList().ForEach(Console.WriteLine);
        }
    }

    public static void printAuthorLines(IEnumerable<AuthorCommitDTO> authorCommits) {
        authorCommits.ToList().ForEach(
            e => {
                Console.WriteLine(e.name);
                printCommitCountLines(e.commits);
                Console.WriteLine(string.Empty);
            }
        );
    }
    
    public static void printCommitCountLines(IEnumerable<CommitCountDTO> commitCounts) {
        commitCounts.ToList().ForEach(e => {
            Console.WriteLine($"{e.count.ToString().PadLeft(6)} {e.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}");
            }
        );
    }
}
