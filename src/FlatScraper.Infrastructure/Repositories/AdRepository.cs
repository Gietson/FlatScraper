using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FlatScraper.Infrastructure.Repositories
{
    public class AdRepository : IAdRepository, IMongoRepository
    {
        private readonly IMongoDatabase _database;

        private IMongoCollection<Ad> Ad => _database.GetCollection<Ad>("Ad");

        public AdRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Ad> GetAsync(Guid id)
            => await Ad.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Ad>> GetAllAsync()
            => await Ad.AsQueryable().ToListAsync();

        public async Task AddAsync(Ad page)
            => await Ad.InsertOneAsync(page);

        public async Task UpdateAsync(Ad scan)
            => await Ad.ReplaceOneAsync(x => x.Id == scan.Id, scan);

        public async Task RemoveAsync(Guid id)
            => await Ad.DeleteOneAsync(x => x.Id == id);
    }
}