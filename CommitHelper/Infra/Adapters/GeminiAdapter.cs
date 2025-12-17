using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommitHelper.Configuration;
using CommitHelper.Domain.Exceptions;

namespace CommitHelper.Infra.Adapters;

public class GeminiAdapter(AiSettings settings) : IAiAdapter
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/"),
        Timeout = TimeSpan.FromSeconds(30)
    };

    public async Task<string> GenerateContentAsync(string prompt, CancellationToken ct = default)
    {
        try
        {
            using var response = await SendGenerationRequestAsync(prompt, ct);

            await EnsureResponseSuccessAsync(response);

            return await ParseResponseContentAsync(response, ct);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new AiAuthenticationException($"API 인증 실패: {ex.Message}", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new AiNetworkException("API 통신 실패 또는 타임아웃 오류.", ex);
        }
        catch (JsonException ex)
        {
            throw new AiRepositoryException($"AI 응답 JSON 파싱 실패: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new AiRepositoryException($"AI 응답 처리 중 알 수 없는 오류 발생: {ex.Message}", ex);
        }
    }

    private Task<HttpResponseMessage> SendGenerationRequestAsync(string prompt, CancellationToken ct)
    {
        string requestUri = $"models/{settings.ModelType}:generateContent?key={settings.ApiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new { role = "user", parts = new[] { new { text = prompt } } }
            },
        };

        return _httpClient.PostAsJsonAsync(
            requestUri,
            requestBody,
            ct);
    }

    private async Task EnsureResponseSuccessAsync(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new UnauthorizedAccessException("API 서버가 인증을 거부했습니다. API Key 또는 권한을 확인하세요.");
        }

        response.EnsureSuccessStatusCode();

        await Task.CompletedTask;
    }

    private static async Task<string> ParseResponseContentAsync(HttpResponseMessage response, CancellationToken ct)
    {
        string rawJson = await response.Content.ReadAsStringAsync(ct);

        using (var document = JsonDocument.Parse(rawJson))
        {
            var candidate = document.RootElement.GetProperty("candidates")[0];
            var textElement = candidate
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text");

            return textElement.GetString() ?? string.Empty;
        }
    }
}
