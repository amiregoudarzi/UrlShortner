using FastEndpoints;

namespace UrlShortner.Api.Modules.Users.Endpoints.Me.Info;

public class Endpoint : Endpoint<Command, Command.Response>
{
 
    public override void Configure()
    {
        Post("/api/users/me/info");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        await ExecuteAsync(req, ct);
    }
}