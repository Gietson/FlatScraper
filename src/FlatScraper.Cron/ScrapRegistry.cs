using System.Threading.Tasks;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.Services;
using FluentScheduler;
using Serilog;

namespace FlatScraper.Cron
{
    public class ScrapRegistry : Registry
    {
        private static readonly ILogger Logger = Log.Logger;
        private static ScraperService _scraperService;

        public ScrapRegistry(IScanPageService scanPageService, IAdRepository adRepository)
        {
            _scraperService = new ScraperService(scanPageService, adRepository);
            Logger.Debug("Execute Scraper Task!");
            Schedule(async () => await Execute()).WithName("Scrap").ToRunNow().AndEvery(15).Minutes();
        }

        private static async Task Execute()
        {
            await _scraperService.ScrapAsync();
        }
    }
}