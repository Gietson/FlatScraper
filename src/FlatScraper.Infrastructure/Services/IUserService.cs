using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Services
{
	public interface IUserService : IService
	{
		Task<UserDto> GetAsync(Guid id);
		Task<UserDto> GetAsync(string email);
		Task<IEnumerable<UserDto>> GetAllAsync();
		Task RegisterAsync(Guid id, string email, string username, string password, string role);
		Task LoginAsync(string email, string password);
		Task RemoveAsync(Guid id);
	}
}