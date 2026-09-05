namespace UrlShortner.Domain.Entities;

public class ShortUrl : Entity
{
    private ShortUrl() { }

    public ShortUrl(string originalUrl, string shortCode)
    {
        OriginalUrl = originalUrl;
        ShortCode = shortCode;
    }

    public string OriginalUrl { get; set; } = null!;
    public string ShortCode { get; set; } = null!;
}
