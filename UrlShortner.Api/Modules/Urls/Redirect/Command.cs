using Dapper;
using DotNetCore.CAP;
using FastEndpoints;
using UrlShortner.Application.Events;
using UrlShortner.Infrastructure.Database;

namespace UrlShortner.Api.Modules.Urls.Redirect
{
    public class Command : ICommand<Response>
    {
        public string? ShortCode { get; init; }
        
        public string? UserAgent { get; set; }

        public string? Referrer { get; set; }
    }

    public sealed class ShortUrlResponse
    {
        public Guid Id { get; init; }
        public string OriginalUrl { get; init; } = string.Empty;
        public string ShortCode { get; init; } = string.Empty;
    }

    public class Response
    {
        public string? OriginalUrl { get; init; } 
    }

    public class CommandHandler(
        ICapPublisher capPublisher,
        IDbConnectionFactory connectionFactory
        ) : ICommandHandler<Command, Response>
    {
        public async Task<Response> ExecuteAsync(Command command, CancellationToken ct)
        {
            await using var connection = await connectionFactory.CreateConnectionAsync(ct);
            const string sql = """
                               SELECT
                                   id AS Id,
                                   original_url AS OriginalUrl,
                                   short_code AS ShortCode
                               FROM dbo.short_urls
                               WHERE short_code = @ShortCode
                               """;
            var dbCommand = new CommandDefinition(
                sql,
                new { command.ShortCode },
                cancellationToken: ct);

            var shortUrl =
                await connection.QuerySingleOrDefaultAsync<ShortUrlResponse>(
                    dbCommand);

            if (shortUrl is null)
                throw new InvalidOperationException("Short URL not found.");

            await capPublisher.PublishAsync(
                "url.clicked",
                new UrlClickedEvent
                {
                    ShortUrlId = shortUrl.Id,
                    UserAgent = command.UserAgent,
                    Referrer = command.Referrer,
                    ClickedAtUtc = DateTime.UtcNow
                },
                cancellationToken: ct);

            return new Response
            {
                OriginalUrl = shortUrl.OriginalUrl
            };
        }
    }
}