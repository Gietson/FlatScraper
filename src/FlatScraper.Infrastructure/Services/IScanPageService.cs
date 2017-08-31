using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Services
{
    public interface IScanPageService : IService
    {
        Task<ScanPageDto> GetAsync(Guid id);
        Task<IEnumerable<ScanPageDto>> GetAllAsync();
        Task AddAsync(ScanPageDto url);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(ScanPageDto page);
    }
}