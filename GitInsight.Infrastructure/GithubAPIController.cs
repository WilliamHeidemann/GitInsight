using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GitInsight.Infrastructure;

public class GithubAPIController {

    private readonly HttpClient _client;

    public GithubAPIController() {
        var config = new ConfigurationBuilder().AddUserSecrets<GithubAPIController>().Build();
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.github.com");
        var token = config["AuthenticationTokens:GitHubAPI"];

        _client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);
    }

    public GithubAPIController(HttpClient MockClient) {
        _client = MockClient;
    }

    public async Task<IEnumerable<ForkDTO>> GetForkList(string owner, string repo) 
    {
        var response = await _client.GetAsync("/repos/" + owner + "/" + repo + "/forks");
        IEnumerable<ForkDTO>? forks = new List<ForkDTO>();

        if(response.IsSuccessStatusCode)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();
            forks = await JsonSerializer.DeserializeAsync<IEnumerable<ForkDTO>>(responseStream);
        }

        return forks!;
    }

    public async Task<bool> IsRepositoryValid(string githubOrganization, string repositoryName)
    {
        var response = await _client.GetAsync($"/repos/{githubOrganization}/{repositoryName}");
        return response.IsSuccessStatusCode;
    }
}