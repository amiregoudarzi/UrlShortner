using Microsoft.EntityFrameworkCore;
using UrlShortner.Domain.Entities;
using UrlShortner.Domain.Repositories;
using UrlShortner.Infrastructure.Infrastructures;

namespace UrlShortner.Infrastructure.Repositories;

public class ShortUrlRepository(AppDbContext dbContext) : IShortUrlRepository
{
    public async Task AddRangeAsync(IEnumerable<ShortUrl> shortUrls, CancellationToken ct = default)
    {
        await dbContext.ShortUrls.AddRangeAsync(shortUrls, ct);
    }
}
