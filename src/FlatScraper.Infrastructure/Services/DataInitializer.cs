using System;
using System.Linq;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using Microsoft.Extensions.Logging;

namespace FlatScraper.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IAdService _adService;
        private readonly ILogger<DataInitializer> _logger;
        private readonly IScanPageService _scanPageService;
        private readonly IScraperService _scraperService;
        private readonly IUserService _userService;

        public DataInitializer(IUserService userService, IScanPageService scanPageService,
            IScraperService scraperService, IAdService adService, ILogger<DataInitializer> logger)
        {
            _userService = userService;
            _scanPageService = scanPageService;
            _scraperService = scraperService;
            _adService = adService;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            var users = await _userService.GetAllAsync();
            if (!users.Any())
            {
                _logger.LogDebug("Initializing users..");
                for (int i = 0; i <= 10; i++)
                {
                    Guid userId = Guid.NewGuid();
                    string username = $"user{i}";

                    _logger.LogTrace($"Adding user: '{username}'.");
                    await _userService.RegisterAsync(userId, $"user{i}@test.com",
                        username, "password", "user");
                }
                for (int i = 0; i <= 3; i++)
                {
                    var userId = Guid.NewGuid();
                    string username = $"admin{i}";
                    _logger.LogTrace($"Adding admin: '{username}'.");
                    await _userService.RegisterAsync(userId, $"admin{i}@test.com", username, "secret", "admin");
                }
            }
            else
                _logger.LogTrace("Users was already initialized.");

            var pages = await _scanPageService.GetAllAsync();
            if (!pages.Any())
            {
                _logger.LogTrace("Initializing scan pages..");
                ScanPageDto page = new ScanPageDto()
                {
                    Active = true,
                    Page = "Gumtree",
                    UrlAddress =
                        "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/v1c9073l3200008p1"
                };
                await _scanPageService.AddAsync(page);

                ScanPageDto pageOlx = new ScanPageDto()
                {
                    Active = true,
                    Page = "Olx",
                    UrlAddress = "https://www.olx.pl/nieruchomosci/mieszkania/sprzedaz/warszawa/"
                };
                await _scanPageService.AddAsync(pageOlx);

                ScanPageDto pageOtodom = new ScanPageDto()
                {
                    Active = true,
                    Page = "Otodom",
                    UrlAddress = "https://www.otodom.pl/sprzedaz/mieszkanie/warszawa/"
                };
                await _scanPageService.AddAsync(pageOtodom);
            }
            else
                _logger.LogTrace("Scan pages was already initialized.");

            var ads = await _adService.GetAllAsync();
            if (!ads.Any())
            {
                _logger.LogTrace($"Scraping...");
                await _scraperService.ScrapAsync(_logger);
            }
            else
                _logger.LogDebug("Scraper was already initialized.");


            _logger.LogDebug("Data was initialized.");
        }
    }
}