using System.Threading.Tasks;

namespace FlatScraper.Infrastructure.Services
{
    public interface IScraperService : IService
    {
        Task ScrapAsync();
    }
}