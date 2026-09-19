using UrlShortner.Application.Interfaces;
using UrlShortner.Domain.Entities;
using UrlShortner.Infrastructure.Infrastructures;

namespace UrlShortner.Infrastructure.Repositories;

public class UrlClickRepository(AppDbContext dbContext)
    : IUrlClickRepository
{
    public async Task AddAsync(
        UrlClick urlClick,
        CancellationToken ct = default)
    {
        await dbContext.UrlClicks.AddAsync(urlClick, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}