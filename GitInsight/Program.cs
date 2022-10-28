// See https://aka.ms/new-console-template for more information

using GitInsight;
using LibGit2Sharp;

string testPath = "./GitTestRepo";
var gitCommitTracker = new GitCommitTracker(testPath);

Console.WriteLine("-------COMMIT FREQUENCY-------");
gitCommitTracker.GetCommitFrequency().ToList().ForEach(Console.WriteLine);

Console.WriteLine("-------AUTHOR COMMITS-------");
gitCommitTracker.GetCommitAuthor().ToList().ForEach(Console.WriteLine);

/*
Console.WriteLine();
Console.WriteLine("-------AUTHOR COMMITS-------");
foreach (var authorcommits in commitlog
             .GroupBy(commit => commit.Author.Email)
             .OrderBy(commits => commits.Key))
{
    Console.WriteLine(authorcommits.Key);
    foreach (var group in authorcommits
                 .GroupBy(commit => commit.Author.When.Date)
                 .OrderBy(commits => commits.Key.Date))
    {
        Console.WriteLine(group.Count() + " " + group.Key.ToShortDateString());
    }
    Console.WriteLine();
}
*/