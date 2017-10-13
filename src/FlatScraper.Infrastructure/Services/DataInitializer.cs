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
	    private readonly IAuthService _authService;
	    private readonly IScanPageService _scanPageService;
		private readonly IScraperService _scraperService;
		private readonly IUserService _userService;

		public DataInitializer(IUserService userService, IScanPageService scanPageService,
			IScraperService scraperService, IAdService adService, IAuthService authService)
		{
			_userService = userService;
			_scanPageService = scanPageService;
			_scraperService = scraperService;
			_adService = adService;
		    _authService = authService;
		}

		public async Task SeedAsync()
		{
			var users = await _userService.GetAllAsync();
			if (!users.Any())
			{
				Logger.Debug("Initializing users..");
				for (int i = 0; i <= 10; i++)
				{
				    string username = $"user{i}";
                    Logger.Debug($"Adding user: '{username}'.");
                    CreateUserDto newUser = new CreateUserDto {Email = $"user{i}@test.com" , Password = "password", Username = username, Role = "user"};

					await _authService.RegisterAsync(newUser);
				}
				for (int i = 0; i <= 3; i++)
				{
					string username = $"admin{i}";
					Logger.Debug($"Adding admin: '{username}'.");
				    CreateUserDto newUser = new CreateUserDto { Email = $"admin{i}@test.com", Password = "secret", Username = username, Role = "admin"};

                    await _authService.RegisterAsync(newUser);
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