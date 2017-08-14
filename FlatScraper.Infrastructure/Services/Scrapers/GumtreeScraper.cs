using FlatScraper.Core.Domain;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
    public class GumtreeScraper : IScraper
    {
        private static decimal PreparePrice(string price)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            string p = digitsOnly.Replace(price, "");

            if (Decimal.TryParse(p, out decimal tempPrice))
                return tempPrice;

            return 0;
        }

        public List<Ad> ParseHomePage(HtmlDocument doc)
        {
            List<Ad> adsList = new List<Ad>();
            HtmlNodeCollection docs = doc.DocumentNode.SelectNodes("//div[@class='container']");
            string host = "https://www.gumtree.pl";
            foreach (HtmlNode ad in docs)
            {
                var nod = ad.SelectSingleNode("div[@class='title']/a");
                
                string title = nod.InnerText.Trim();
                string url = host + nod.Attributes["href"].Value;
                string idAds = url.Split('/').Last();

                var priceTemp = ad.SelectSingleNode("div[@class='info']/div[@class='price']/span[@class='value']/span[@class='amount']");
                decimal price = PreparePrice(priceTemp?.InnerText);

                Ad ads = Ad.Create(Guid.NewGuid(), title, url, price, host);

                adsList.Add(ads);
            }

            return adsList;
        }

        public AdDetails ParseDetailsPage(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

    }
}
