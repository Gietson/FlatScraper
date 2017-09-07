using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FlatScraper.Infrastructure.Services
{
    public interface IScraperService : IService
    {
        Task ScrapAsync(ILogger logger);
    }
}