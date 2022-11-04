namespace GitInsight.Core;

public interface IPersistentStorage{
    IEnumerable<DbCommitDTO> FindAllCommits(string FilePath);
}