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
            Logger.Debug("Start Scraper Task!");
            Schedule(async () => await new ScraperService(scanPageService, adRepository).ScrapAsync()).ToRunNow().AndEvery(2).Minutes();
        }
    }
}
