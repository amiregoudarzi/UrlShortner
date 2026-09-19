namespace UrlShortner.Application.Events;

public sealed class UrlClickedEvent
{
    public Guid ShortUrlId { get; init; }

    public DateTime ClickedAtUtc { get; init; }

    public string? UserAgent { get; init; }

    public string? Referrer { get; init; }
}