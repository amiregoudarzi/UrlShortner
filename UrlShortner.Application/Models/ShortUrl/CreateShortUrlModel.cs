namespace UrlShortner.Application.Models.ShortUrl;

public class CreateShortUrlModel
{
    public string OriginalUrl { get; init; } = string.Empty;
    public string ShortCode { get; init; } = string.Empty;
}