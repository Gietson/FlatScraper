using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.Services;
using FluentScheduler;
using Serilog;

namespace FlatScraper.Cron
{
    public class ScrapJob : IJob
    {
        private readonly IScanPageService _scanPageService;
        private readonly IAdRepository _adRepository;
        private static readonly ILogger Logger = Log.Logger;

        public ScrapJob(IScanPageService scanPageService, IAdRepository adRepository)
        {
            _scanPageService = scanPageService;
            _adRepository = adRepository;
        }
        public async void Execute()
        {
            Logger.Debug("Execute Scraper Task!");
            await new ScraperService(_scanPageService, _adRepository).ScrapAsync();
        }
    }
}
