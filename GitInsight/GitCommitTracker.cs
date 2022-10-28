using System.Globalization;

namespace GitInsight;
public class GitCommitTracker 
{
    private readonly Repository _repository;

    public GitCommitTracker(string path)
    {
        _repository = Repository.IsValid(path) ? new Repository(path) : throw new ArgumentException("Repository was not found.");
    }

    // public IEnumerable<string> GetCommitFrequency() 
    //     => GetCommitFrequencyByAuthor(_repository.Commits);

    public IEnumerable<string> GetCommitAuthor() 
    {
        
        foreach (var authorcommits in _repository.Commits
             .ToList()
             .GroupBy(commit => commit.Committer.Name)
             .Distinct())
        {        

            yield return authorcommits.FirstOrDefault()!.Committer.Name;
            foreach (var commit in authorcommits
                    .GroupBy(r => r.Committer.When.Date)
                    .Select(g => new {count = g.Count(), date = g.First().Committer.When.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)})) {
                var space = commit.count > 9 ? "    " : "     ";
                yield return space + commit.count+ " " + commit.date;
            }
            yield return "";
        }
    }

    public IEnumerable<string> GetCommitFrequency() {
         foreach (var commitsByDay in _repository.Commits
             .GroupBy(commit => commit.Author.When.Date)
             .OrderBy(commits => commits.Key.Date))
        {
            var space = commitsByDay.Count() > 9 ? "    " : "     ";
            yield return space + commitsByDay.Count() + " " + commitsByDay.Key.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}