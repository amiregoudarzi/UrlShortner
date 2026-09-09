using UrlShortner.Application.Models.ShortUrl;

namespace UrlShortner.Application.Services.ShortUrl;

public interface IShortUrlService
{
    Task<List<CreateShortUrlModel>> CreateShortCodes(List<string> originalUrls, CancellationToken ct);
}