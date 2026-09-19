using UrlShortner.Domain.Entities;

namespace UrlShortner.Application.Interfaces;

public interface IUrlClickRepository
{
    Task AddAsync(
        UrlClick urlClick,
        CancellationToken ct = default);
}