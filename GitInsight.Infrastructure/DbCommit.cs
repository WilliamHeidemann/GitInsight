namespace GitInsight.Infrastructure;

public class DbCommit
{
    //[Required]
    //[Key]
    public string SHA {get; set;} = null!;

    //[Required]
    //[MaxLength(50)]
    public string AuthorName {get; set;} = null!;

    // [Required]
    public DateTime Date {get; set;}

    // [Required]
    public int RepoId {get; set;}
}