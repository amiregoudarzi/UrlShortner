using DotNetCore.CAP;
using UrlShortner.Application.Interfaces;
using UrlShortner.Domain.Entities;

namespace UrlShortner.Application.Events.Handlers;

public class UrlClickedEventHandler(IUrlClickRepository repository) : ICapSubscribe
{
    [CapSubscribe(
        "url.clicked",
        Group = "url.analytics")]
    public async Task HandleAsync(
        UrlClickedEvent message,
        CancellationToken cancellationToken)
    {
        var click = new UrlClick(
            message.ShortUrlId,
            message.UserAgent,
            message.Referrer);

        await repository.AddAsync(
            click,
            cancellationToken);
    }
}