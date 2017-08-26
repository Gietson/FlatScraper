using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Core.Domain;

namespace FlatScraper.Core.Repositories
{
    public interface IAdRepository : IRepository
    {
        Task<Ad> GetAsync(Guid id);
        Task<IEnumerable<Ad>> GetAllAsync();
        Task AddAsync(Ad ad);
        Task UpdateAsync(Ad ad);
        Task RemoveAsync(Guid id);
    }
}