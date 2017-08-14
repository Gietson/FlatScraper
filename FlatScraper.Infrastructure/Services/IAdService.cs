using FlatScraper.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatScraper.Infrastructure.Services
{
    public interface IAdService
    {
        Task<AdDto> GetAsync(Guid id);
        Task<IEnumerable<AdDto>> GetAllAsync();
        Task AddAsync(string url);
    }
}
