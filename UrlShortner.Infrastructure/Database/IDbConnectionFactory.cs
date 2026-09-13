using System.Data.Common;

namespace UrlShortner.Infrastructure.Database;

public interface IDbConnectionFactory
{
    Task<DbConnection> CreateConnectionAsync(CancellationToken ct = default);
}