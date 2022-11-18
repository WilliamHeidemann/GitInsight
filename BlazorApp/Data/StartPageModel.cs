namespace BlazorApp.Data;
using System.ComponentModel.DataAnnotations;

public class StartPageModel
{
    [Required]
    public string? RepoPath { get; set; }
}