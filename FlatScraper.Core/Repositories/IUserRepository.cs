using FlatScraper.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatScraper.Core.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task RemoveAsync(Guid id);
    }
}
