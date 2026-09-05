using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Infrastructure.Infrastructures;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((_, options) =>
{
    options.ListenAnyIP(3030);
});

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(options =>
    {
        options.DocumentSettings = settings =>
        {
            settings.Title = "UrlShortner API";
            settings.Version = "v1";
        };
    });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseFastEndpoints();

app.Run();