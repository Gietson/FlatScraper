using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;

namespace FlatScraper.Infrastructure.Repositories
{
    public class InMemoryScanPageRepository : IScanPageRepository
    {
        private static readonly ISet<ScanPage> _scanPages = new HashSet<ScanPage>();

        public async Task AddAsync(ScanPage page)
            => await Task.FromResult(_scanPages.Add(page));

        public async Task<IEnumerable<ScanPage>> GetAllAsync()
            => await Task.FromResult(_scanPages);

        public async Task<ScanPage> GetAsync(Guid id)
            => await Task.FromResult(_scanPages.SingleOrDefault(x => x.Id == id));

        public async Task UpdateAsync(ScanPage page)
        {
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(ScanPage page)
        {
            await Task.FromResult(_scanPages.Remove(page));
        }
    }
}