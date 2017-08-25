using System;
using System.Collections.Generic;
using System.Linq;
using FlatScraper.Core.Domain;
using FlatScraper.Infrastructure.Extensions;
using HtmlAgilityPack;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
    public class GumtreeScraper : IScraper
    {
        public List<Ad> ParseHomePage(HtmlDocument doc)
        {
            List<Ad> adsList = new List<Ad>();
            HtmlNodeCollection docs = doc.DocumentNode.SelectNodes("//div[@class='container']");
            string host = "https://www.gumtree.pl";
            foreach (HtmlNode ad in docs)
            {
                HtmlNode nod = ad.SelectSingleNode("div[@class='title']/a");

                string title = nod.InnerText.Trim();
                string url = host + nod.Attributes["href"].Value;
                string idAds = url.Split('/').Last();

                HtmlNode priceTemp =
                    ad.SelectSingleNode(
                        "div[@class='info']/div[@class='price']/span[@class='value']/span[@class='amount']");
                decimal price = ScrapExtensions.PreparePrice(priceTemp?.InnerText);

                Ad ads = Ad.Create(Guid.NewGuid(), idAds, title, url, price, host);

                adsList.Add(ads);
            }

            return adsList;
        }

        public AdDetails ParseDetailsPage(HtmlDocument doc)
        {
            return null;
        }
    }
}