using FlatScraper.Core.Domain;
using FlatScraper.Infrastructure.DTO;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
    public interface IScraper
    {
        List<Ad> ParseHomePage(HtmlDocument doc);
        AdDetails ParseDetailsPage(HtmlDocument doc);
    }
}
