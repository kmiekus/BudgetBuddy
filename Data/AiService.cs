using System.Net.Http.Headers;
using System.Text.Json;

namespace BudgetBuddy.Data;

public interface IAiService
{
    Task<string> AskAsync(string prompt);
}

public class OpenRouterAiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public OpenRouterAiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["OpenRouter:ApiKey"] ?? throw new ArgumentNullException(_apiKey, "OpenRouter:ApiKey configuration is missing.");
        _model = config["OpenRouter:Model"] ?? throw new ArgumentNullException(_model, "OpenRouter:Model configuration is missing.");
    }

    public async Task<string> AskAsync(string prompt)
    {
        var body = new
        {
            model = _model,
            messages = new[]
            {
            new { role = "user", content = prompt }
        }
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsJsonAsync("https://openrouter.ai/api/v1/chat/completions", body);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()!;
    }
}