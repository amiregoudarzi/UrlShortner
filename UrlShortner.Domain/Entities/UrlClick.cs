namespace UrlShortner.Domain.Entities;

public class UrlClick : Entity
{
    public UrlClick(
                    Guid shortUrlId,
                    string? userAgent,
                    string? referrer
                    )
    {
        ShortUrlId = shortUrlId;
        UserAgent = userAgent;
        Referrer = referrer;
    }
    
    public Guid ShortUrlId { get; private set; }
    
    public string? UserAgent { get; private set; }

    public string? Referrer { get; private set; }
}