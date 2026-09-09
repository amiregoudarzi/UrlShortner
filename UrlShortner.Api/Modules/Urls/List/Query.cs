using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Application.Interfaces;
using UrlShortner.Infrastructure.Infrastructures;

namespace UrlShortner.Api.Modules.Urls.List;

public sealed record Query : ICommand<IReadOnlyList<Query.Response>>
{
    public sealed record Response
    {
        public Guid Id { get; init; }

        public string Url { get; init; } = string.Empty;

        public string ShortUrl { get; init; } = string.Empty;
    }

    private sealed class Handler(AppDbContext dbContext, IRedisCacheService redis)
        : ICommandHandler<Query, IReadOnlyList<Response>>
    {
        private const string CacheKey = "short-urls:all";

        public async Task<IReadOnlyList<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var cached = await redis.GetAsync<IReadOnlyList<Response>>(
                CacheKey,
                ct);

            if (cached is not null)
                return cached;

            var urls = await dbContext.ShortUrls
                .AsNoTracking()
                .Select(u => new Response
                {
                    Id = u.Id,
                    Url = u.OriginalUrl,
                    ShortUrl = u.ShortCode
                })
                .ToListAsync(ct);

            await redis.SetAsync(
                CacheKey,
                urls,
                ct);

            return urls;
        }
    }
}