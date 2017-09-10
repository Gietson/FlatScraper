using System.Threading.Tasks;

namespace FlatScraper.Core.Repositories
{
	public interface IScaperRepository : IRepository
	{
		Task ScrapAsync();
	}
}