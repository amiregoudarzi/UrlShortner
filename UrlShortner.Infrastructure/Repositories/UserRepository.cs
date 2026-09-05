using Microsoft.EntityFrameworkCore;
using UrlShortner.Domain.Entities;
using UrlShortner.Domain.Repositories;
using UrlShortner.Infrastructure.Infrastructures;

namespace UrlShortner.Infrastructure.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await dbContext.Users.AddAsync(user, ct);
    }

    public Task<User?> FindAsync(Guid userId, CancellationToken ct = default)
    {
        return dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId, ct);
    }
}
