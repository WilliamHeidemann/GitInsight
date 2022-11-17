using System.ComponentModel.DataAnnotations;

namespace GitInsight.Core;

public record CommitCountDTO(int count, DateTime Date);