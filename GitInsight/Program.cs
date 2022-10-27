// See https://aka.ms/new-console-template for more information

using LibGit2Sharp;

Repository repository = new(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent + "\\GitTestRepo");
var commitlog = repository.Commits;

Console.WriteLine("-------COMMIT FREQUENCY-------");
foreach (var group in commitlog
             .GroupBy(commit => commit.Author.When.Date)
             .OrderBy(commits => commits.Key.Date))
{
    Console.WriteLine(group.Count() + " " + group.Key.ToShortDateString());
}

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