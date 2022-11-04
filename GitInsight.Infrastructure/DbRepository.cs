namespace GitInsight.Infrastructure;

public class DbRepository 
{
    [Required]
    [Key]
    public string Filepath {get; init;}

    public DbCommit NewestCommit {get; set;}
}