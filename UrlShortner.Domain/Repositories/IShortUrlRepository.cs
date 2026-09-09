using UrlShortner.Domain.Entities;

namespace UrlShortner.Domain.Repositories;

public interface IShortUrlRepository
{
    Task AddRangeAsync(IEnumerable<ShortUrl> shortUrls, CancellationToken ct = default);
}
