namespace GitInsight.Infrastructure;

public class DbCommit
{
    [Required]
    [Key]
    public string SHA {get; set;}

    [Required]
    [MaxLength(50)]
    public string AuthorName {get; set;}

    [Required]
    public DateTime Date {get; set;}

    public DbCommit? ParentCommit {get; set;}
}