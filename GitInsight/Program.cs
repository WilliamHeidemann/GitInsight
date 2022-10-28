// See https://aka.ms/new-console-template for more information

using GitInsight;

string testPath = "./GitTestRepo";
var gitCommitTracker = new GitCommitTracker(testPath);

Console.WriteLine("-------COMMIT FREQUENCY-------");
gitCommitTracker.GetCommitFrequency().ToList().ForEach(Console.WriteLine);

Console.WriteLine("-------AUTHOR COMMITS-------");
gitCommitTracker.GetCommitAuthor().ToList().ForEach(Console.WriteLine);