using FastEndpoints;

namespace UrlShortner.Api.Modules.Urls.Get.List;

public sealed class Endpoint : EndpointWithoutRequest<IReadOnlyList<Query.Response>>
{
    public override void Configure()
    {
        Get("/api/urls");
        AllowAnonymous();
        // Options(builder => builder.RequireRateLimiting("discovery"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await new Query().ExecuteAsync(ct);
        await Send.OkAsync(response, ct);
    }

    private sealed class EndpointSummary : Summary<Endpoint>
    {
        public EndpointSummary()
        {
            Summary = "List registered urls";
            Description = "List registered urls.";
            Response<IReadOnlyList<Query.Response>>(200, "urls list");
        }
    }
}
