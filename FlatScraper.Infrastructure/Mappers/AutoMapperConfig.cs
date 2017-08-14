using AutoMapper;
using FlatScraper.Core.Domain;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Mappers
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserDto, User>().ReverseMap();
                    cfg.CreateMap<AdDto, Ad>().ReverseMap();
                })
                .CreateMapper();
    }
}