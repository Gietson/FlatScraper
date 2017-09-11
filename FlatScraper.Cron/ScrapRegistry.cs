using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.Services;
using FluentScheduler;

namespace FlatScraper.Cron
{
    public class ScrapRegistry : Registry
    {
        public ScrapRegistry(IScanPageService scanPageService, IAdRepository adRepository)
        {
            Schedule(async () => await new ScraperService(scanPageService, adRepository).ScrapAsync()).ToRunNow().AndEvery(2).Minutes();
        }
    }
}
