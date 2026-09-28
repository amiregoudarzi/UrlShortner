namespace UrlShortner.Blazor.Services.Urls;

public sealed class GetListUrl(IHttpClientFactory httpClientFactory)
    : ApiService(httpClientFactory)
{
    public async Task<List<Response>> ExecuteAsync(
        CancellationToken ct = default)
    {
        var result = await HttpClient.GetFromJsonAsync<List<Response>>(
            "api/urls",
            ct);

        return result ?? [];
    }

    public sealed class Response
    {
        public Guid Id { get; init; }

        public string Url { get; init; } = string.Empty;

        public string ShortUrl { get; init; } = string.Empty;
    }
}