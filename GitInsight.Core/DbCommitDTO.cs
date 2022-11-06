using System.ComponentModel.DataAnnotations;

namespace GitInsight.Core;

public record DbCommitDTO(string SHA, string AuthorName, DateTime Date);

public record DbCommitCreateDTO(string SHA, [MaxLength(50)]string AuthorName, DateTime Date);
