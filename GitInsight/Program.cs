using LibGit2Sharp;

using var test = new Repository(Repository.Clone(@"https://github.com/WilliamHeidemann/GitInsight", "./test"));

foreach (var commit in test.Commits) {
    Console.WriteLine(commit.Author.Name);
}

var sig = new Signature("Andreas", "aguh@itu.dk", DateTimeOffset.Now);

test.Commit("Test message", sig, sig);
test.Commit("Second commit", sig, sig);

foreach (var commit in test.Commits) {
    Console.WriteLine(commit.Author.Name);
}


