using UrlShortner.Domain.Entities;

namespace UrlShortner.Application.Services;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken ct = default);
    
    Task<User?> FindAsync(Guid userId, CancellationToken ct = default);
}
