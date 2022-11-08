namespace GitInsight.Infrastructure;

public class DbRepository 
{
    [Required]
    [Key]
    public int Id {get; init;}

    [Required]
    public string FilePath { get; init; } = null!;

    public string? NewestCommitSHA { get; init; }
}