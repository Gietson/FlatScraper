using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;

namespace FlatScraper.Infrastructure.Repositories
{
	public class InMemoryAdRepository : IAdRepository
	{
		private static readonly ISet<Ad> _ad = new HashSet<Ad>();

		public async Task AddAsync(Ad ad)
		{
			_ad.Add(ad);
			await Task.CompletedTask;
		}

		public async Task<IEnumerable<Ad>> GetAllAsync()
			=> await Task.FromResult(_ad);

		public async Task<Ad> GetAsync(Guid id)
			=> await Task.FromResult(_ad.SingleOrDefault(x => x.Id == id));

		public async Task RemoveAsync(Guid id)
		{
			var user = await GetAsync(id);
			_ad.Remove(user);
			await Task.CompletedTask;
		}

		public async Task UpdateAsync(Ad ad)
		{
			await Task.CompletedTask;
		}
	}
}