using FastEndpoints;
using Microsoft.EntityFrameworkCore;
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

    private sealed class Handler(AppDbContext dbContext) : ICommandHandler<Query, IReadOnlyList<Response>>
    {
      
        public async Task<IReadOnlyList<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            return await dbContext.ShortUrls.Select(u => new Response()
            {
                Id = u.Id,
                Url = u.OriginalUrl,
                ShortUrl = u.ShortCode
            }).ToListAsync(ct);
        }
    }
}
