namespace GitInsight.Core;

public record GitCommitInfoDTO(DateTime timestamp, GitCommitAuthorDTO author, GitCommitStatsDTO stats);

public record GitCommitStatsDTO(int total, int additions, int deletions);

public record GitCommitAuthorDTO(GitAuthorDTO author);

public record GitAuthorDTO(string name, DateTime date);