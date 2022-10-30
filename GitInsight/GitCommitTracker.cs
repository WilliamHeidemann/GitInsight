using System.Globalization;

namespace GitInsight;
public class GitCommitTracker 
{
    private readonly Repository _repository;

    public GitCommitTracker(string path)
    {
        _repository = Repository.IsValid(path) ? new Repository(path) : throw new ArgumentException($"Repository was not found at {path}.");
    }
    
    public IEnumerable<string> GetCommitAuthor()
    {
        foreach (var authorcommits in _repository.Commits
                     .GroupBy(commit => commit.Committer.Name)
                     .Distinct())
        {
            yield return authorcommits.First().Committer.Name;
            foreach (var commitInfo in GetCommitFrequency(authorcommits))
            {
                yield return commitInfo;
            }
            yield return string.Empty;
        }
    }

    public IEnumerable<string> GetCommitFrequency() => GetCommitFrequency(_repository.Commits);
    
    private IEnumerable<string> GetCommitFrequency(IEnumerable<Commit> commitLog)
    {
        return from commit in commitLog
            .GroupBy(commit => commit.Committer.When.Date)
            .OrderBy(commits => commits.Key.Date)
            let space = commit.Count() > 9 ? "    " : "     " 
            select $"{space}{commit.Count()} {commit.Key.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";
    }
}