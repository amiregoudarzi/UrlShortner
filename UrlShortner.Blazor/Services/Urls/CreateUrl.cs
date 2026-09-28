namespace UrlShortner.Blazor.Services.Urls;

public sealed class CreateUrl(IHttpClientFactory httpClientFactory)
    : ApiService(httpClientFactory)
{
    public async Task<List<string>> CreateShortUrlsAsync(
        List<string> urls,
        CancellationToken ct = default)
    {
        var response = await HttpClient.PostAsJsonAsync(
            "api/urls",
            new CreateShortUrlsRequest
            {
                Urls = urls
            },
            ct);

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<CreateShortUrlsResponse>(ct);

        return result?.ShortUrls ?? [];
    }

    private sealed class CreateShortUrlsRequest
    {
        public List<string> Urls { get; init; } = [];
    }

    private sealed class CreateShortUrlsResponse
    {
        public List<string> ShortUrls { get; init; } = [];
    }
}