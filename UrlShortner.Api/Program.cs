using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Mapping;
using UrlShortner.Application.Services.ShortUrl;
using UrlShortner.Domain.Repositories;
using UrlShortner.Infrastructure.Infrastructures;
using UrlShortner.Infrastructure.Repositories;
using UrlShortner.Infrastructure.UnitOfWork;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ShortUrlProfile>();
});

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

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IShortUrlService, ShortUrlService>();
builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseFastEndpoints();

app.Run();