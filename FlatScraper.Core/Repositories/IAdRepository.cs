using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Core.Domain;

namespace FlatScraper.Core.Repositories
{
    public interface IAdRepository
    {
        Task<Ad> GetAsync(Guid id);
        Task<IEnumerable<Ad>> GetAllAsync();
        Task AddAsync(Ad ad);
        Task UpdateASync(Ad ad);
        Task RemoveAsync(int id);
    }
}