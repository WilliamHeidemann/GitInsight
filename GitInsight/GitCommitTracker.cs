using System.Globalization;
using GitInsight.Core;
/*
namespace GitInsight;
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

    public IEnumerable<DbCommitDTO> FindAllCommits(string filePath)
    {
        if(!Repository.IsValid(filePath)) throw new RepositoryNotFoundException("The Repository does not exist!");
        var (response, newestCommit) = FindNewestCommit(filePath);
        if (response == Response.NotFound)
        {
            Create(new DbRepositoryCreateDTO(filePath));
        }
        if (!IsUpToDate(filePath, newestCommit)) 
        {
            Update(filePath);
        }
        (_, newestCommit) = FindNewestCommit(filePath);
        var commits = FindAllCommitsFromNewestCommit(newestCommit);
        
        return ConvertUpdateDTO2DTO(commits); 
    }

}
*/