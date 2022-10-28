using System.Globalization;

namespace GitInsight;
public class GitCommitTracker 
{
    private readonly Repository _repository;

    public GitCommitTracker(string path)
    {
        _repository = Repository.IsValid(path) ? new Repository(path) : throw new ArgumentException("Repository was not found.");
    }

    public IEnumerable<string> GetCommitFrequency() 
        => GetCommitFrequencyByAuthor(_repository.Commits);

    public IEnumerable<string> GetCommitAuthor() {
        foreach (var authorcommits in _repository.Commits
             .GroupBy(commit => commit.Author.Email)
             .OrderBy(commits => commits.Key))
        {        
            yield return authorcommits.Key;
            foreach (var commit in GetCommitFrequencyByAuthor(authorcommits)) {
                yield return commit;
            }
            yield return "";
        }
    }

    private IEnumerable<string> GetCommitFrequencyByAuthor(IEnumerable<Commit> authorCommits) {
         foreach (var commitsByDay in _repository.Commits
             .GroupBy(commit => commit.Author.When.Date)
             .OrderBy(commits => commits.Key.Date))
        {
            yield return commitsByDay.Count() + " " + commitsByDay.Key.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}