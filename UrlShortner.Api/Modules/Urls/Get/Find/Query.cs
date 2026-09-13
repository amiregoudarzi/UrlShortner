using Dapper;
using FastEndpoints;
using UrlShortner.Application.Interfaces;
using UrlShortner.Infrastructure.Database;

namespace UrlShortner.Api.Modules.Urls.Get.Find;

public sealed record Query : ICommand<IReadOnlyList<Query.Response>>
{
    [QueryParam] public int Page { get; init; } = 1;
    [QueryParam] public int PageSize { get; init; } = 20;
    [QueryParam] public string? Search { get; init; }


    public sealed record Response
    {
        public Guid Id { get; init; }

        public string Url { get; init; } = string.Empty;

        public string ShortUrl { get; init; } = string.Empty;
    }

    private sealed class Handler(IDbConnectionFactory connectionFactory, IRedisCacheService redis)
        : ICommandHandler<Query, IReadOnlyList<Response>>
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(
            Query query,
            CancellationToken ct)
        {
            // =========================
            // No search
            // =========================
            if (query.Search is null)
            {
                var cacheKey =
                    $"short-urls:page:{query.Page}:size:{query.PageSize}";

                // Only cache first 3 pages
                if (query.Page <= 3)
                {
                    var cached = await redis.GetAsync<IReadOnlyList<Response>>(
                        cacheKey,
                        ct);

                    if (cached is not null)
                        return cached;
                }

                await using var connection = await connectionFactory.CreateConnectionAsync(ct);

                const string sql = """
                                   SELECT
                                       id AS Id,
                                       original_url AS Url,
                                       short_code AS ShortUrl
                                   FROM dbo.short_urls
                                   ORDER BY created_date_utc DESC
                                   OFFSET @Offset ROWS
                                   FETCH NEXT @PageSize ROWS ONLY
                                   """;

                var command = new CommandDefinition(
                    sql,
                    new
                    {
                        Offset = (query.Page - 1) * query.PageSize,
                        query.PageSize
                    },
                    cancellationToken: ct);

                var urls = (await connection.QueryAsync<Response>(command))
                    .ToList();
                if (query.Page <= 3)
                {
                    await redis.SetAsync(
                        cacheKey,
                        urls,
                        ct);
                }
                
                return urls;
            }

            // =========================
            // Search
            // =========================

            var searchCacheKey = $"short-urls:search:{query.Search}";

            var searchCached = await redis.GetAsync<IReadOnlyList<Response>>(
                searchCacheKey,
                ct);

            if (searchCached is not null)
            {
                return searchCached
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToList();
            }
            
            await using var searchConnection = await connectionFactory.CreateConnectionAsync(ct);

            const string searchSql = """
                                     SELECT
                                         id AS Id,
                                         original_url AS Url,
                                         short_code AS ShortUrl
                                     FROM dbo.short_urls
                                     WHERE OriginalUrl LIKE @Search
                                     ORDER BY created_date_utc DESC
                                     """;

            var searchCommand = new CommandDefinition(
                searchSql,
                new
                {
                    Search = $"%{query.Search}%"
                },
                cancellationToken: ct);

            var searchResults = (await searchConnection.QueryAsync<Response>(searchCommand)).ToList();

            if (searchResults.Count > 0)
            {
                await redis.SetAsync(
                    searchCacheKey,
                    searchResults,
                    ct);
            }

            return searchResults
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();
        }
    }
}