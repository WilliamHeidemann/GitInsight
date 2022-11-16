using System.Text.Json;
using System.Text.Json.Serialization;

namespace GitInsight.Infrastructure;

public class GithubAPIController {

    private readonly IConfiguration _config;

    public GithubAPIController(IConfiguration config) {
        _config = config;

    }
    public async Task<IEnumerable<Fork>?> GetForkList(string owner, string repo) 
    {
        //Credit for setup: https://gist.github.com/MaximRouiller/74ae40aa994579393f52747e78f26441

        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://api.github.com");
        var token = _config["AuthenticationTokens:GitHubAPI"];

        client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

        var response = await client.GetAsync("/repos/" + owner + "/" + repo + "/forks");
        IEnumerable<Fork>? forks = new List<Fork>();

        if(response.IsSuccessStatusCode)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();
            forks = await JsonSerializer.DeserializeAsync<IEnumerable<Fork>>(responseStream);
        }
        return forks;
    }

    public class Fork 
    {
        [JsonPropertyName("full_name")]
        public string? name {get; set;}
    }
}