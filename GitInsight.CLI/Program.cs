// See https://aka.ms/new-console-template for more information
using CommandLine;
using GitInsight;
using GitInsight.Core;
using Microsoft.Extensions.Configuration;
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
        
        IEnumerable<DbCommitDTO> commitsToAnalyze;
        try 
        {
            commitsToAnalyze = await persistentStorageController.FindAllCommitsAsync(input.Value.RepoPath);
        } 
        catch (RepositoryNotFoundException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
         
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
