using FastEndpoints;

namespace UrlShortner.Api.Modules.Urls.Get.Find;

public sealed class Endpoint : Endpoint<Query, IReadOnlyList<Query.Response>>
{
    public override void Configure()
    {
        Get("/api/urls/find");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        var response = await req.ExecuteAsync(ct);

        await Send.OkAsync(response, ct);
    }

    private sealed class EndpointSummary : Summary<Endpoint>
    {
        public EndpointSummary()
        {
            Summary = "List registered urls";
            Description = "List registered urls.";
            Response<IReadOnlyList<List.Query.Response>>(200, "urls list");
        }
    }
}