using FastEndpoints;

namespace UrlShortner.Api.Modules.Users.Endpoints.Me.Info;

public class Command : ICommand<Command.Response>
{
    public class Response()
    {
        
    }
    
    public class CommandHandler() : ICommandHandler<Command, Command.Response>
    {
        public async Task<Response> ExecuteAsync(Command command, CancellationToken ct)
        {
            return null;
        }
    }
}


