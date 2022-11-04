namespace GitInsight.Infrastructure;

public class DbRepository 
{
    [Required]
    [Key]
    public string FilePath {get; init;}

    public DbCommit? NewestCommit {get; set;}
}