using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Common.Mongo;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Services
{
	public interface IAdService : IService
	{
		Task<AdDto> GetAsync(Guid id);
		Task<IEnumerable<AdDto>> GetAllAsync();
	    Task<PagedResult<AdDto>> BrowseAsync();
		Task AddAsync(AdDto adDto);
	}
}