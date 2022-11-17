using System.Globalization;
using GitInsight.Core;

namespace GitInsight.CLI;
public class GitCommitTracker
{
    public IEnumerable<string> GetCommitFrequency(IEnumerable<DbCommitDTO> commitsToAnalyze)
    {
        return from commit in commitsToAnalyze
                .GroupBy(commit => commit.Date.Day)
            let padding = commit.Count() > 9 ? "    " : "     "
            let amount = commit.Count()
            let date = commit.First().Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            select $"{padding}{amount} {date}";
    }

    public IEnumerable<string> GetCommitAuthor(IEnumerable<DbCommitDTO> commitsToAnalyze)
    {
        foreach (var authorcommits in commitsToAnalyze
                     .GroupBy(commit => commit.AuthorName)
                     .Distinct())
        {
            yield return authorcommits.First().AuthorName;
            foreach (var commitInfo in GetCommitFrequency(authorcommits))
            {
                yield return commitInfo;
            }
            yield return string.Empty;
        }
    }
}
