using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Application.Interfaces;
using UrlShortner.Application.Mapping;
using UrlShortner.Application.Services.ShortUrl;
using UrlShortner.Domain.Repositories;
using UrlShortner.Infrastructure.Caching;
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

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IShortUrlService, ShortUrlService>();
builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseFastEndpoints();

app.Run();