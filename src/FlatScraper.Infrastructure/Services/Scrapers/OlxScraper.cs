using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
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
            DateTime createAt = DateTime.MinValue;
            string district = null;
            string city = null;
            string typeOfProperty = null;
            string parking = null;
            bool agency = false;
            int numberOfRooms = 0;
            int numberOfBathrooms = 0;
            int size = 0;


            HtmlNode details = doc.DocumentNode.SelectSingleNode(
                "//div[@class='offer-titlebox'] / div[@class='offer-titlebox__details']");

            if (details == null)
            {
                //scrap otodom
                return null;
            }

            var locationTemp = details.SelectSingleNode("a").InnerText;
            var location = locationTemp.Split(",");
            city = location[0];
            district = location[2];


            var createAtTemp = details.SelectSingleNode("em").InnerText.Trim();
            var regexBeforeChar = Regex.Replace(createAtTemp, "^[^_]*o ", "");
            var regexAfterChar = Regex.Replace(regexBeforeChar, ", ID.*$", "");
            createAt =
                DateTime.ParseExact(regexAfterChar, "hh:mm, d MMMM yyyy", CultureInfo.CreateSpecificCulture("pl-PL"));

            decimal priceM2 = ad.Price / size;

            string username = "";

            AdDetails adDetails = AdDetails.Create(
                priceM2,
                district,
                city,
                agency,
                typeOfProperty,
                numberOfRooms,
                numberOfBathrooms,
                size,
                username,
                new List<string>(),
                createAt);
                
            return null;
        }
    }
}