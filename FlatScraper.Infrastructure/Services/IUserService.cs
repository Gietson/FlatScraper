using FlatScraper.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlatScraper.Infrastructure.Services
{
    public interface IUserService : IService
    {
        Task<UserDto> GetAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task RegisterAsync(Guid id, string email, string username, string password, string role);
        Task LoginAsync(string email, string password);
    }
}