using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using UrlShortner.Application.Interfaces;

namespace UrlShortner.Infrastructure.Caching;

public class RedisCacheService(IDistributedCache cache) : IRedisCacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        try
        {
            var value = await cache.GetStringAsync(key, ct);

            if (value is null) return default;

            return JsonSerializer.Deserialize<T>(value);
        }
        catch (Exception e)
        {
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken ct = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(value);

            await cache.SetStringAsync(
                key,
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                },
                ct);
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}