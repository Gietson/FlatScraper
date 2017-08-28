using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlatScraper.Core.Domain;

namespace FlatScraper.Core.Repositories
{
    public interface IScaperRepository : IRepository
    {
        Task ScrapAsync();
    }
}
