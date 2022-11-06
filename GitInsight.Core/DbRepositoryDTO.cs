namespace GitInsight.Core;

public record DbRepositoryDTO(string Filepath, string NewestCommit);

public record DbRepositoryUpdateDTO(string Filepath);

public record DbRepositoryCreateDTO(string Filepath);