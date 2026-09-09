using UrlShortner.Application.Interfaces;
using UrlShortner.Infrastructure.Infrastructures;

namespace UrlShortner.Infrastructure.UnitOfWork;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return dbContext.SaveChangesAsync(ct);
    }
}