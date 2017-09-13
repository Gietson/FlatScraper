using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.Services;
using FluentScheduler;
using Serilog;

namespace FlatScraper.Cron
{
    public class ScrapRegistry : Registry
    {
        private static readonly ILogger Logger = Log.Logger;

        public ScrapRegistry(IScanPageService scanPageService, IAdRepository adRepository)
        {
            Logger.Debug("Execute Scraper Task!");
            Schedule(async () => await new ScraperService(scanPageService, adRepository).ScrapAsync()).WithName("Scrap").ToRunNow().AndEvery(15).Minutes();
        }
    }
}
