using FastEndpoints;
using UrlShortner.Domain.Entities;

namespace UrlShortner.Api.Modules.Urls.Create;

public class Command : ICommand<Response>
{
    public string Url { get; init; } = string.Empty;
}

public class Response
{
    public string ShortUrl { get; init; } = string.Empty;
}

public class CommandHandler : ICommandHandler<Command, Response>
{
    public async Task<Response> ExecuteAsync(
        Command command,
        CancellationToken ct)
    {
        var code = GenerateCode();
        
        return new Response
        {
            ShortUrl = code
        };
        
    }
    
    private static string GenerateCode(int length = 6)
    {
        const string chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return string.Concat(
            Enumerable.Range(0, length)
                .Select(_ => chars[Random.Shared.Next(chars.Length)])
        );
    }
}