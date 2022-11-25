namespace GitInsight.Infrastructure.Tests;

using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moq;

public class GithubAPIControllerTests {

    public GithubAPIController Setup(IEnumerable<ForkDTO> expectedAPIResponse, HttpStatusCode statusCode)
    {
        var JsonString = JsonSerializer.Serialize(expectedAPIResponse);
        var content = new StringContent(JsonString, Encoding.UTF8, "application/json");
        
        var GithubAPIMock = new HttpClient(new HttpMessageHandlerMock(new HttpResponseMessage()
        {
            StatusCode = statusCode,
            Content = content,
        }));
        GithubAPIMock.BaseAddress = new Uri("https://api.github.com");
        return new GithubAPIController(GithubAPIMock);
    }

    [Fact]
    public async Task GetForkList_Returns_Empty_For_Repo_With_No_Forks() 
    {
        // Arrange
        var githubResponse = new List<ForkDTO>(){};
        var githubAPIController = Setup(githubResponse, HttpStatusCode.OK);

        string owner = "OliFryser";
        string repo = "GitInsightTestRepo_NoForks";

        // Act
        var result = await githubAPIController.GetForkList(owner, repo);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetForkList_Returns_1_Fork_For_Repo_With_1_Fork() 
    {
        // Arrange
        var githubResponse = new List<ForkDTO>()
        {
            new("ForkedRepo", "https://github.com/A-Guldborg/ForkedRepo", new("A-Guldborg", "https://avatars.githubusercontent.com/u/95026056?s=80&v=4"))
        };
        var githubAPIController = Setup(githubResponse, HttpStatusCode.OK);
        string owner = "OliFryser";
        string repo = "GitInsightTestRepo_1Fork";

        // Act
        var result = await githubAPIController.GetForkList(owner, repo);

        //Assert
        result.First().name.Should().Be("ForkedRepo");
    }

    [Fact]
    public async void Valid_Repoistory_Should_Return_True()
    {
        // Given
        var githubResponse = new List<ForkDTO>()
        {
            new("ForkedRepo", "https://github.com/A-Guldborg/ForkedRepo", new("A-Guldborg", "https://avatars.githubusercontent.com/u/95026056?s=80&v=4"))
        };
        var githubAPIController = Setup(githubResponse, HttpStatusCode.OK);
        string owner = "itu-bdsa";
        string repo = "project-description";
    
        // When
        var result = await githubAPIController.IsRepositoryValid(owner, repo);
    
        // Then
        result.Should().Be(true);
    }

    [Fact]
    public async void Invalid_Repository_Should_Return_False()
    {
        // Given
        var githubResponse = new List<ForkDTO>(){};
        var githubAPIController = Setup(githubResponse, HttpStatusCode.BadRequest);
        string owner = "WilliamHeidemann";
        string repo = "InsightGit";
    
        // When
        var result = await githubAPIController.IsRepositoryValid(owner, repo);
    
        // Then
        result.Should().Be(false);
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

    [Fact]
    public void GetCommitSHAs_Returns_List_Of_SHA_Strings()
    {
        // Given

        string owner = "OliFryser";
        string repo = "GitInsightTestRepo_NoForks";
    
        // When
    
        // Then
    }
}