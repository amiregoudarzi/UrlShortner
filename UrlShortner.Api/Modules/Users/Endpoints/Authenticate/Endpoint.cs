using FastEndpoints;

namespace UrlShortner.Api.Modules.Users.Endpoints.Authenticate;

public class Endpoint : Endpoint<Command, Command.Response>
{
 
    public override void Configure()
    {
        Post("/api/users/authenticate");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(Command command, CancellationToken ct)
    {
        await ExecuteAsync(command, ct);
    }
}