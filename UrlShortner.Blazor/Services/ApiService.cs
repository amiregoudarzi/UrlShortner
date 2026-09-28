namespace UrlShortner.Blazor.Services;

public abstract class ApiService
{
    protected readonly HttpClient HttpClient;

    protected ApiService(IHttpClientFactory httpClientFactory)
    {
        HttpClient = httpClientFactory.CreateClient("Api");
    }
}