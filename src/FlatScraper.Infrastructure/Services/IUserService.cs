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
		Task RemoveAsync(Guid id);
	}
}