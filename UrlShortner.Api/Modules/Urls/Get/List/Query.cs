using FastEndpoints;
using Dapper;
using UrlShortner.Application.Interfaces;
using UrlShortner.Infrastructure.Database;

namespace UrlShortner.Api.Modules.Urls.Get.List;

public sealed record Query : ICommand<IReadOnlyList<Query.Response>>
{
    public sealed record Response
    {
        public Guid Id { get; init; }

        public string Url { get; init; } = string.Empty;

        public string ShortUrl { get; init; } = string.Empty;
    }

    private sealed class Handler(
        IRedisCacheService redis,
        IDbConnectionFactory connectionFactory)
        : ICommandHandler<Query, IReadOnlyList<Response>>
    {
        private const string CacheKey = "short-urls:all";

        public async Task<IReadOnlyList<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var cached = await redis.GetAsync<IReadOnlyList<Response>>(
                CacheKey,
                ct);

            if (cached is not null) return cached;
            
            await using var connection = await connectionFactory.CreateConnectionAsync(ct);
            
            const string sql = """
                               SELECT
                                   id AS Id,
                                   original_url AS Url,
                                   short_code AS ShortUrl
                               FROM dbo.short_urls
                               """;

            var urls = (await connection.QueryAsync<Response>(sql)).ToList();

            await redis.SetAsync(
                CacheKey,
                urls,
                ct);

            return urls;
        }
    }
}