using UrlShortner.Domain.Entities;

namespace UrlShortner.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken ct = default);
    
    Task<User?> FindAsync(Guid userId, CancellationToken ct = default);
}
