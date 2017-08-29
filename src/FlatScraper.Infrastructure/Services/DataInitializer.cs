﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using NLog;

namespace FlatScraper.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        //private readonly IAdService _adService;
        private readonly IScanPageService _scanPageService;

        private readonly IScraperService _scraperService;
        private readonly IUserService _userService;

        public DataInitializer(IUserService userService, IScanPageService scanPageService, IScraperService scraperService)
        {
            _userService = userService;
            _scanPageService = scanPageService;
            _scraperService = scraperService;
        }

        public async Task SeedAsync()
        {
            var users = await _userService.GetAllAsync();
            if (users.Any())
            {
                Logger.Trace("Data was already initialized.");
                return;
            }
            Logger.Trace("Initializing data..");

            for (int i = 0; i <= 10; i++)
            {
                Guid userId = Guid.NewGuid();
                string username = $"user{i}";

                Logger.Trace($"Adding user: '{username}'.");
                await _userService.RegisterAsync(userId, $"user{i}@test.com",
                    username, "password", "user");
            }
            for (int i = 0; i <= 3; i++)
            {
                var userId = Guid.NewGuid();
                string username = $"admin{i}";
                Logger.Trace($"Adding admin: '{username}'.");
                await _userService.RegisterAsync(userId, $"admin{i}@test.com", username, "secret", "admin");
            }

            ScanPageDto page = new ScanPageDto()
            {
                Active = true,
                Page = "Gumtree",
                UrlAddress = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/v1c9073l3200008p1"
            };
            await _scanPageService.AddAsync(page);

            ScanPageDto pageOlx = new ScanPageDto()
            {
                Active = true,
                Page = "Olx",
                UrlAddress = "https://www.olx.pl/nieruchomosci/mieszkania/sprzedaz/warszawa/"
            };
            await _scanPageService.AddAsync(pageOlx);

            Logger.Debug($"Scraping...");
            await _scraperService.ScrapAsync();

            Logger.Trace("Data was initialized.");
        }
    }
}