using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FlatScraper.Infrastructure.Repositories
{
	public class ScanPageRepository : IScanPageRepository, IMongoRepository
	{
		private readonly IMongoDatabase _database;

		private IMongoCollection<ScanPage> ScanPage => _database.GetCollection<ScanPage>("ScanPage");

		public ScanPageRepository(IMongoDatabase database)
		{
			_database = database;
		}

		public async Task<ScanPage> GetAsync(Guid id)
			=> await ScanPage.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);

		public async Task<IEnumerable<ScanPage>> GetAllAsync()
			=> await ScanPage.AsQueryable().ToListAsync();

		public async Task AddAsync(ScanPage page)
			=> await ScanPage.InsertOneAsync(page);

		public async Task UpdateAsync(ScanPage scan)
			=> await ScanPage.ReplaceOneAsync(x => x.Id == scan.Id, scan);

		public async Task RemoveAsync(ScanPage page)
			=> await ScanPage.DeleteOneAsync(x => x.Id == page.Id);
	}
}