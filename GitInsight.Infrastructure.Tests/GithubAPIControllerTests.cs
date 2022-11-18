namespace GitInsight.Infrastructure.Tests;

using Microsoft.Extensions.Configuration;

public class GithubAPIControllerTests {
    
    private readonly GithubAPIController _githubAPIController;

    public GithubAPIControllerTests() {
        _githubAPIController = new GithubAPIController();
    }

    [Fact]
    public async Task GetForkList_Returns_Null_For_Repo_With_No_Forks() 
    {
        // Arrange
        string owner = "OliFryser";
        string repo = "GitInsightTestRepo_NoForks";

        // Act
        var result = await _githubAPIController.GetForkList(owner, repo);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetForkList_Returns_1_Fork_For_Repo_With_1_Fork() 
    {
        // Arrange
        string owner = "OliFryser";
        string repo = "GitInsightTestRepo_1Fork";

        // Act
        var result = await _githubAPIController.GetForkList(owner, repo);

        //Assert
        foreach (var item in result) {
            item.name.Should().Be("A-Guldborg/ForkedRepo");
        }
    }

    [Fact (Skip = "Github Actions workflow does not contain API authentication key")]
    public async void Valid_Repoistory_Should_Return_True()
    {
        // Given
        string owner = "itu-bdsa";
        string repo = "project-description";
    
        // When
        var result = await _githubAPIController.IsRepositoryValid(owner, repo);
    
        // Then
        result.Should().Be(true);
    }

    [Fact (Skip = "Github Actions workflow does not contain API authentication key")]
    public async void Invalid_Repository_Should_Return_False()
    {
        // Given
        string owner = "WilliamHeidemann";
        string repo = "InsightGit";
    
        // When
        var result = await _githubAPIController.IsRepositoryValid(owner, repo);
    
        // Then
        result.Should().Be(false);
    }
}