namespace GitInsight.Infrastructure;

public class DbRepository 
{
    [Required]
    [Key]
    public int Id {get; set;}

    [Required]
    public string FilePath { get; set; }

    public string? NewestCommitSHA { get; set; }

    public DbRepository(string filePath) 
    {
        this.FilePath = filePath;
    }
}