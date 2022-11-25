namespace GitInsight.BlazorApp.Data;
using System.ComponentModel.DataAnnotations;

public class StartPageModel
{
    public bool IsValidRepo { get; set; }
    [Required]
    [MinLength(1, ErrorMessage = "RepoPath must not be empty")]
    public string? RepoPath { get; set; }
}