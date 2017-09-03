using System;
using System.Collections.Generic;
using FlatScraper.Core.Domain;
using FlatScraper.Infrastructure.Extensions;
using HtmlAgilityPack;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
    public class OlxScraper : IScraper
    {
        public List<Ad> ParseHomePage(HtmlDocument doc)
        {
            List<Ad> adsList = new List<Ad>();
            HtmlNodeCollection docs = doc.DocumentNode.SelectNodes("// tbody / tr[@class='wrap'] / td");
            string host = "https://www.olx.pl";

            foreach (HtmlNode ad in docs)
            {
                HtmlNode nod = ad.SelectSingleNode("table / tbody / tr[1]");

                string url = nod.SelectSingleNode("td[1] / a").Attributes["href"].Value;

                string title = nod.SelectSingleNode("td[2] / div / h3 / a / strong").InnerText.Trim();

                string idAds = ad.SelectSingleNode("table").Attributes["data-id"].Value;

                HtmlNode priceTemp = nod.SelectSingleNode("td[3] / div / p / strong");

                decimal price = ScrapExtensions.PreparePrice(priceTemp?.InnerText);

                Ad ads = Ad.Create(Guid.NewGuid(), idAds, title, url, price, host);

                adsList.Add(ads);
            }

            return adsList;
        }

        public AdDetails ParseDetailsPage(HtmlDocument doc, Ad ad)
        {
            return null;
        }
    }
}