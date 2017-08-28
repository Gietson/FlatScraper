using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Services
{
    public interface IAdService : IService
    {
        Task<AdDto> GetAsync(Guid id);
        Task<IEnumerable<AdDto>> GetAllAsync();
        Task AddAsync(AdDto adDto);
    }
}