namespace GitInsight.Infrastructure.Tests;

using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moq;

public class GithubAPIControllerTests {
    [Fact]
    public async Task GetForkList_Returns_Empty_For_Repo_With_No_Forks() 
    {
        // Arrange
        string owner = "OliFryser";
        string repo = "GitInsightTestRepo_NoForks";

        var githubAPIController = new GithubAPIController();


        // Act
        var result = await githubAPIController.GetForkList(owner, repo);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetForkList_Returns_1_Fork_For_Repo_With_1_Fork() 
    {
        // Arrange
        string owner = "OliFryser";
        string repo = "GitInsightTestRepo_1Fork";

        var githubAPIController = new GithubAPIController();

        // Act
        var result = await githubAPIController.GetForkList(owner, repo);

        //Assert
        result.First().name.Should().Be("ForkedRepo");
    }

    [Fact]
    public async void Valid_Repoistory_Should_Return_True()
    {
        // Given
        string owner = "itu-bdsa";
        string repo = "project-description";
    
        var githubAPIController = new GithubAPIController();
    
        // When
        var result = await githubAPIController.IsRepositoryValid(owner, repo);
    
        // Then
        result.Should().Be(true);
    }

    [Fact]
    public async void Invalid_Repository_Should_Return_False()
    {
        // Given
        string owner = "WilliamHeidemann";
        string repo = "InsightGit";

        var githubAPIController = new GithubAPIController();
    
        // When
        var result = await githubAPIController.IsRepositoryValid(owner, repo);
    
        // Then
        result.Should().Be(false);
    }

    [Fact]
    public async void GetCommitSHAs_Returns_List_Of_SHA_Strings()
    {
        // Given
        string owner = "OliFryser";
        string repo = "GitInsightTestRepo_NoForks";
    
        var githubAPIController = new GithubAPIController();

        // When
        var result = await githubAPIController.GetCommitSHAs(owner, repo);
    
        // Then
        result.Count().Should().Be(1);
        result.First().sha.Should().Be("5dd5878a4d94757be50c8c6c7f9ebf92a42169c1");
    }


    public class HttpMessageHandlerMock : HttpMessageHandler
    {
        private readonly HttpResponseMessage _httpResponseMessage;

        public HttpMessageHandlerMock(HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_httpResponseMessage);
        }
    }
}