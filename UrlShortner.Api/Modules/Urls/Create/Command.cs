using FastEndpoints;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Services.ShortUrl;
using UrlShortner.Domain.Entities;
using UrlShortner.Domain.Repositories;

namespace UrlShortner.Api.Modules.Urls.Create;

public class Command : ICommand<Response>
{
    public List<string> Urls { get; init; } = [];
}

public class Response
{
    public List<string>? ShortUrls { get; init; } = [];
}

public class CommandHandler (
    IShortUrlService urlService, 
    IUnitOfWork unitOfWork,
    IShortUrlRepository repository,
    AutoMapper.IMapper mapper) : ICommandHandler<Command, Response>
{
    public async Task<Response> ExecuteAsync(
        Command command,
        CancellationToken ct)
    {
        if (command.Urls.Count == 0) return new Response();

        var models = await urlService.CreateShortCodes(command.Urls, ct);

        var entities = mapper.Map<List<ShortUrl>>(models);
        
        await repository.AddRangeAsync(entities, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new Response
        {
            ShortUrls = entities
                .Select(x => x.ShortCode)
                .ToList()
        };
    }
}