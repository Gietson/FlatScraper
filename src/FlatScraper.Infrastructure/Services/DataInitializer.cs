﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace FlatScraper.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAdService _adService;
        private readonly IUserService _userService;

        public DataInitializer(IUserService userService, IAdService adService)
        {
            _userService = userService;
            _adService = adService;
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

            string url = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/v1c9073l3200008p1";
            Logger.Trace($"Initializing ads, url = {url}");
            await _adService.AddAsync(url);


            Logger.Trace("Data was initialized.");
        }
    }
}