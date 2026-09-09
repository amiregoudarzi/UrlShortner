using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Models.ShortUrl;

namespace UrlShortner.Application.Services.ShortUrl;

public class ShortUrlService : IShortUrlService
{
    public Task<List<CreateShortUrlModel>> CreateShortCodes(List<string> originalUrls, CancellationToken ct)
    {
        const int length = 6;
        const string fallbackCharacters = "abcdefghijklmnopqrstuvwxyz";

        var result = new List<CreateShortUrlModel>(originalUrls.Count);

        foreach (var originalUrl in originalUrls)
        {
            var characters = originalUrl
                .Where(char.IsLetterOrDigit)
                .ToArray();

            var characterPool = new string(characters);

            if (characterPool.Length < length)
                characterPool += fallbackCharacters;

            var shortCode = string.Concat(
                Enumerable.Range(0, length)
                    .Select(_ => characterPool[
                        Random.Shared.Next(characterPool.Length)
                    ])
            );

            result.Add(new CreateShortUrlModel
            {
                OriginalUrl = originalUrl,
                ShortCode = shortCode
            });
        }

        return Task.FromResult(result);
    }
}