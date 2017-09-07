using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IAdRepository _adRepository;
        private readonly IScanPageService _scanPageService;
        private IScraper _scraper;

        public ScraperService(IScanPageService scanPageService, IAdRepository adRepository)
        {
            _scanPageService = scanPageService;
            _adRepository = adRepository;
        }

        public async Task ScrapAsync()
        {
            IEnumerable<Type> scraperTypes = ScrapExtensions.GetScraperTypes();

            var scanPages = await _scanPageService.GetAllAsync();
            var adsDb = await _adRepository.GetAllAsync();

            foreach (ScanPageDto scanPage in scanPages)
            {
                Type scrapClass = scraperTypes
                    .FirstOrDefault(x => x.Name.ToLower()
                        .Replace("Scraper", "")
                        .Contains(scanPage.Page.ToLower()));
                if (scrapClass == null)
                {
                    throw new Exception(
                        $"Invalid scan page, UrlAddress='{scanPage.UrlAddress}', Page='{scanPage.Page}'.");
                }

                _scraper = Activator.CreateInstance(scrapClass) as IScraper;

                HtmlDocument scrapedDoc = ScrapExtensions.ScrapUrl(scanPage.UrlAddress);
                if (scrapedDoc == null)
                {
                    throw new Exception(
                        $"Problem with scrap page = '{scanPage.UrlAddress}', scrapClass='{scrapClass.Name}'.");
                }

                List<Ad> ads = _scraper.ParseHomePage(scrapedDoc);

                foreach (Ad ad in ads)
                {
                    bool isInDb = adsDb.Any(x => x.IdAds == ad.IdAds);
                    if (!isInDb)
                    {
                        HtmlDocument scrapedSubPage = ScrapExtensions.ScrapUrl(ad.Url);
                        ad.AdDetails = _scraper.ParseDetailsPage(scrapedSubPage, ad);

                        await _adRepository.AddAsync(ad);
                    }
                }
            }
        }
    }
}