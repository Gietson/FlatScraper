using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Services
{
    public interface IAdService
    {
        Task<AdDto> GetAsync(Guid id);
        Task<IEnumerable<AdDto>> GetAllAsync();
        Task AddAsync(string url);
    }
}