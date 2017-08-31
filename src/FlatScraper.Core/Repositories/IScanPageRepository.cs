using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Core.Domain;

namespace FlatScraper.Core.Repositories
{
    public interface IScanPageRepository : IRepository
    {
        Task<ScanPage> GetAsync(Guid id);
        Task<ScanPage> GetAsync(string urlAddress);
        Task<IEnumerable<ScanPage>> GetAllAsync();
        Task AddAsync(ScanPage page);
        Task UpdateAsync(ScanPage scan);
        Task RemoveAsync(ScanPage page);
    }
}