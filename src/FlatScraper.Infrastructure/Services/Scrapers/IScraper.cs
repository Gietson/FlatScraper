using System.Collections.Generic;
using FlatScraper.Core.Domain;
using FlatScraper.Infrastructure.DTO;
using HtmlAgilityPack;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
    public interface IScraper
    {
        List<Ad> ParseHomePage(HtmlDocument doc, ScanPageDto scanPage);
        AdDetails ParseDetailsPage(HtmlDocument doc, Ad ad);
    }
}