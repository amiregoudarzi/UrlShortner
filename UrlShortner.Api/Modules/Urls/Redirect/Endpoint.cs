using FastEndpoints;

namespace UrlShortner.Api.Modules.Urls.Redirect;

public sealed class Endpoint : Endpoint<Command, Response>
{
    public override void Configure()
    {
        Get("api/{shortCode}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        req.UserAgent = HttpContext.Request.Headers.UserAgent.ToString();

        req.Referrer = HttpContext.Request.Headers.Referer.ToString();
        
        var response = await req.ExecuteAsync(ct);

        await Send.OkAsync(response, ct);
    }
}