
namespace UrlShortner.Blazor.Services.Urls;

public sealed class FindListUrl(IHttpClientFactory httpClientFactory)
    : ApiService(httpClientFactory)
{
    public async Task<List<Response>> ExecuteAsync(
        Request request,
        CancellationToken ct = default)
    {
        var query = new List<string>
        {
            $"page={request.Page}",
            $"pageSize={request.PageSize}"
        };

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query.Add($"search={Uri.EscapeDataString(request.Search)}");
        }

        var url = $"api/urls/find?{string.Join("&", query)}";

        var result = await HttpClient.GetFromJsonAsync<List<Response>>(
            url,
            ct);

        return result ?? [];
    }

    public sealed class Request
    {
        public int Page { get; init; } = 1;

        public int PageSize { get; init; } = 20;

        public string? Search { get; init; }
    }

    public sealed class Response
    {
        public Guid Id { get; init; }

        public string Url { get; init; } = string.Empty;

        public string ShortUrl { get; init; } = string.Empty;
    }
}