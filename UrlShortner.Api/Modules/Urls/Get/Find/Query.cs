using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Application.Interfaces;
using UrlShortner.Infrastructure.Infrastructures;

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

    private sealed class Handler(AppDbContext dbContext, IRedisCacheService redis)
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

                var urls = await dbContext.ShortUrls
                    .AsNoTracking()
                    .OrderByDescending(x => x.CreatedDateUtc)
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(u => new Response
                    {
                        Id = u.Id,
                        Url = u.OriginalUrl,
                        ShortUrl = u.ShortCode
                    })
                    .ToListAsync(ct);

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

            var searchCacheKey =
                $"short-urls:search:{query.Search}";

            var searchCached =
                await redis.GetAsync<IReadOnlyList<Response>>(
                    searchCacheKey,
                    ct);

            if (searchCached is not null)
            {
                return searchCached
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToList();
            }

            var searchResults = await dbContext.ShortUrls
                .AsNoTracking()
                .Where(x => x.OriginalUrl.Contains(query.Search))
                .OrderByDescending(x => x.CreatedDateUtc)
                .Select(u => new Response
                {
                    Id = u.Id,
                    Url = u.OriginalUrl,
                    ShortUrl = u.ShortCode
                })
                .ToListAsync(ct);

            await redis.SetAsync(
                searchCacheKey,
                searchResults,
                ct);

            return searchResults
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();
        }
    }
}

// todo : if the list of search was null, do not cache the search