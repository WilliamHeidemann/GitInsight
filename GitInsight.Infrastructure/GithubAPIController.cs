using System.Text.Json;
using System.Text.Json.Serialization;

namespace GitInsight.Infrastructure;

public class GithubAPIController {

    private readonly IConfiguration _config;
    private readonly HttpClient _client;

    public GithubAPIController(IConfiguration config) {
        _config = config;
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.github.com");
        var token = _config["AuthenticationTokens:GitHubAPI"];

        _client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

    }
    public async Task<IEnumerable<Fork>> GetForkList(string owner, string repo) 
    {
        var response = await _client.GetAsync("/repos/" + owner + "/" + repo + "/forks");
        IEnumerable<Fork>? forks = new List<Fork>();

        if(response.IsSuccessStatusCode)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();
            forks = await JsonSerializer.DeserializeAsync<IEnumerable<Fork>>(responseStream);
        }

        return forks!;
    }

    public class Fork 
    {
        [JsonPropertyName("full_name")]
        public string? name {get; set;}
    }
}