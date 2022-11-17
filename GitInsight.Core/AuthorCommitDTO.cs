using System.ComponentModel.DataAnnotations;

namespace GitInsight.Core;

public record AuthorCommitDTO(IEnumerable<CommitCountDTO> commits, string name);