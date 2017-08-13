using FlatScraper.Infrastructure.DTO;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
    public class GumtreeScraper
    {
        public List<AdDto> ParseHomePage(HtmlDocument doc, string host)
        {
            List<AdDto> _adsList = new List<AdDto>();
            HtmlNodeCollection docs = doc.DocumentNode.SelectNodes("//div[@class='container']");
            AdDto ads;

            foreach (HtmlNode ad in docs)
            {
                ads = new AdDto();

                var nod = ad.SelectSingleNode("div[@class='title']/a");
                
                ads.Title = nod.InnerText.Trim();
                ads.Url = host + nod.Attributes["href"].Value;
                ads.IdAds = ads.Url.Split('/').Last();

                var price = ad.SelectSingleNode("div[@class='info']/div[@class='price']/span[@class='value']/span[@class='amount']");
                ads.Price = PreparePrice(price?.InnerText);

                _adsList.Add(ads);
            }

            return _adsList;
        }

        private static decimal PreparePrice(string price)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            string p = digitsOnly.Replace(price, "");

            if (Decimal.TryParse(p, out decimal tempPrice))
                return tempPrice;

            return 0;
        }
    }
}
