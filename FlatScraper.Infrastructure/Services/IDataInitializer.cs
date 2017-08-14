using System.Threading.Tasks;

namespace FlatScraper.Infrastructure.Services
{
    public interface IDataInitializer : IService
    {
        Task SeedAsync();
    }
}