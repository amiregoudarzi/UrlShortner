using FastEndpoints;

namespace UrlShortner.Api.Modules.Urls.Create;

public sealed class Endpoint : Endpoint<Command, Response>
{
    public override void Configure()
    {
        Post("/api/urls");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        var response = await req.ExecuteAsync(ct);

        await Send.OkAsync(response, ct);
        
    }
}