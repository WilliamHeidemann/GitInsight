namespace GitInsight.Infrastructure;

public class DbRepository 
{
    [Required]
    [Key]
    public string FilePath { get; init; } = null!;

    public DbCommit? NewestCommit { get; init; }
}