using AutoMapper;
using UrlShortner.Application.Models.ShortUrl;
using UrlShortner.Domain.Entities;

namespace UrlShortner.Application.Mapping;

public class ShortUrlProfile : Profile
{
    public ShortUrlProfile()
    {
        CreateMap<CreateShortUrlModel, ShortUrl>();
    }
}