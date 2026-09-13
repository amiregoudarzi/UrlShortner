using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace UrlShortner.Infrastructure.Database;

public class SqlConnectionFactory(IConfiguration configuration)
    : IDbConnectionFactory
{
    public async Task<DbConnection> CreateConnectionAsync(
        CancellationToken ct = default)
    {
        var connection = new SqlConnection(
            configuration.GetConnectionString("SqlConnection"));

        await connection.OpenAsync(ct);

        return connection;
    }
}