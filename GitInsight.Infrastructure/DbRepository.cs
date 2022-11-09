namespace GitInsight.Infrastructure;

public class DbRepository 
{
    [Required]
    [Key]
    public int Id {get; init;}

    [Required]
    public string FilePath { get; init; }

    public string? NewestCommitSHA { get; set; }

    public DbRepository(string filePath) 
    {
        this.FilePath = filePath;
    }
}