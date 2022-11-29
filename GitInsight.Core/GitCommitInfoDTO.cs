namespace GitInsight.Core;

public record GitCommitInfoDTO(GitCommitDTO commit, GitCommitStatsDTO stats);

public record CommitSHA(string sha);

public record GitCommitStatsDTO(int total, int additions, int deletions);

public record GitCommitDTO(GitCommitAuthorDTO author);

public record GitCommitAuthorDTO(string name, DateTime date);