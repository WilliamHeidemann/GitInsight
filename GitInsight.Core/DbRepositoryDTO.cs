namespace GitInsight.Core;

public record DbRepositoryDTO(int RepoId, string Filepath, string NewestCommitSha);

public record DbRepositoryUpdateDTO(string FilePath);

public record DbRepositoryCreateDTO(string Filepath);