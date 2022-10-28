// See https://aka.ms/new-console-template for more information

using GitInsight;

public class Program
{
    public static void Main(string[] args)
    {
        GitCommitTracker gitCommitTracker;
        if (args.Length == 0)
        {
            Console.WriteLine("You have to give me a path!");
            System.Environment.Exit(0);
        }

        gitCommitTracker = new GitCommitTracker(args[0]);

        if (args.Length > 1 && args[1] == "author")
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
