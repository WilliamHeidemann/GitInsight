namespace GitInsight.Core;

public record DbRepositoryDTO(int RepoId, string Filepath, string NewestCommitSha);

public record DbRepositoryUpdateDTO(int RepoId, string FilePath);

public record DbRepositoryCreateDTO(string Filepath);