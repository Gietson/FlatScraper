using System;
using System.Linq;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using Serilog;

namespace FlatScraper.Infrastructure.Services
{
	public class DataInitializer : IDataInitializer
	{
		private static readonly ILogger Logger = Log.Logger;
		private readonly IAdService _adService;
		private readonly IScanPageService _scanPageService;
		private readonly IScraperService _scraperService;
		private readonly IUserService _userService;

		public DataInitializer(IUserService userService, IScanPageService scanPageService,
			IScraperService scraperService, IAdService adService)
		{
			_userService = userService;
			_scanPageService = scanPageService;
			_scraperService = scraperService;
			_adService = adService;
		}

		public async Task SeedAsync()
		{
			var users = await _userService.GetAllAsync();
			if (!users.Any())
			{
				Logger.Debug("Initializing users..");
				for (int i = 0; i <= 10; i++)
				{
					Guid userId = Guid.NewGuid();
					string username = $"user{i}";

					Logger.Debug($"Adding user: '{username}'.");
					await _userService.RegisterAsync(userId, $"user{i}@test.com",
						username, "password", "user");
				}
				for (int i = 0; i <= 3; i++)
				{
					var userId = Guid.NewGuid();
					string username = $"admin{i}";
					Logger.Debug($"Adding admin: '{username}'.");
					await _userService.RegisterAsync(userId, $"admin{i}@test.com", username, "secret", "admin");
				}
			}
			else
				Logger.Debug("Users was already initialized.");

			var pages = await _scanPageService.GetAllAsync();
			if (!pages.Any())
			{
				Logger.Debug("Initializing scan pages..");
				ScanPageDto page = new ScanPageDto()
				{
					Active = true,
					Host = "Gumtree",
                    HostUrl = "https://www.gumtree.pl",
                    UrlAddress =
						"https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/v1c9073l3200008p1"
				};
				await _scanPageService.AddAsync(page);

				ScanPageDto pageOlx = new ScanPageDto()
				{
					Active = true,
					Host = "Olx",
                    HostUrl = "https://www.olx.pl",
                    UrlAddress = "https://www.olx.pl/nieruchomosci/mieszkania/sprzedaz/warszawa/"
				};
				await _scanPageService.AddAsync(pageOlx);

				ScanPageDto pageOtodom = new ScanPageDto()
				{
					Active = false,
					Host = "Otodom",
                    HostUrl = "https://www.otodom.pl",
                    UrlAddress = "https://www.otodom.pl/sprzedaz/mieszkanie/warszawa/"
				};
				await _scanPageService.AddAsync(pageOtodom);
			}
			else
				Logger.Debug("Scan pages was already initialized.");

			var ads = await _adService.GetAllAsync();
			if (!ads.Any())
			{
				Logger.Debug($"Scraping...");
				await _scraperService.ScrapAsync();
			}
			else
				Logger.Debug("Scraper was already initialized.");


			Logger.Debug("Data was initialized.");
		}
	}
}