using AutoMapper;
using FlatScraper.Common.Mongo;
using FlatScraper.Core.Domain;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Mappers
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, UserDto>().ReverseMap();
                    cfg.CreateMap<Ad, AdDto>().ReverseMap();
                    cfg.CreateMap<ScanPage, ScanPageDto>().ReverseMap();
                    cfg.CreateMap<AdDetails, AdDetailsDto>().ReverseMap();
                    cfg.CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
                })
                .CreateMapper();
    }
}