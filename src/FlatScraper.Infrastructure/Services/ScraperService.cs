using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.DTO;
using FlatScraper.Infrastructure.Extensions;
using FlatScraper.Infrastructure.Services.Scrapers;
using HtmlAgilityPack;

namespace FlatScraper.Infrastructure.Services
{
    public class ScraperService : IScraperService
    {
        private readonly IScanPageService _scanPageService;
        private IScraper _scraper;
        private readonly IAdRepository _adRepository;

        public ScraperService(IScanPageService scanPageService, IAdRepository adRepository)
        {
            _scanPageService = scanPageService;
            _adRepository = adRepository;
        }
        public async Task ScrapAsync()
        {
            Type type = typeof(IScraper);
            Type[] assembly = Assembly.GetAssembly(type).GetTypes();

            IEnumerable<Type> scraperTypes = assembly.Where(x =>
                x.GetInterfaces().Contains(typeof(IScraper)) && x.GetConstructor(Type.EmptyTypes) != null);


            var scanPages = await _scanPageService.GetAllAsync();
            foreach (ScanPageDto scanPage in scanPages)
            {
                Type scrapClass = scraperTypes.FirstOrDefault(x => x.Name.ToLower().Replace("Scraper", "").Contains(scanPage.UrlAddress.ToLower()));
                if (scrapClass == null)
                {
                    throw new Exception($"Invalid scan page, url='{scanPage}'.");
                }

                _scraper = Activator.CreateInstance(scrapClass) as IScraper;

                HtmlDocument scrapedDoc = ScrapExtensions.ScrapUrl(scanPage.UrlAddress);
                List<Ad> ads = _scraper.ParseHomePage(scrapedDoc);

                foreach (Ad ad in ads)
                {
                    HtmlDocument scrapedSubPage = ScrapExtensions.ScrapUrl(ad.Url);
                    ad.AdDetails = _scraper.ParseDetailsPage(scrapedSubPage);

                    await _adRepository.AddAsync(ad);
                }
            }

            
        }
    }
}
