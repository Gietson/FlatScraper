using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FlatScraper.Core.Domain;
using FlatScraper.Infrastructure.Extensions;
using HtmlAgilityPack;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
    public class OtodomScraper : IScraper
    {
        public List<Ad> ParseHomePage(HtmlDocument doc)
        {
            List<Ad> adsList = new List<Ad>();
            HtmlNodeCollection docs = doc.DocumentNode.SelectNodes("//div[@class='row'] / div / article");
            string host = "https://www.otodom.pl";
            foreach (HtmlNode ad in docs)
            {
                var url = ad.SelectSingleNode("div[@class='offer-item-details'] / header / h3 / a").Attributes["href"]
                    .Value;
                var title = ad.SelectSingleNode("div[@class='offer-item-details'] / header / h3 / a / span / span")
                    .InnerText.Trim();

                string idAds = ad.Attributes["data-tracking-id"].Value;

                var priceTemp =
                    ad.SelectSingleNode("div[@class='offer-item-details'] / ul / li[@class='offer-item-price']")
                        .InnerText.Trim();

                decimal price = ScrapExtensions.PreparePrice(priceTemp);

                Ad ads = Ad.Create(Guid.NewGuid(), idAds, title, url, price, host);

                adsList.Add(ads);
            }

            return adsList;
        }

        public AdDetails ParseDetailsPage(HtmlDocument doc, Ad ad)
        {
            DateTime createAt = DateTime.UtcNow;
            string district = null;
            string city = null;
            string typeOfProperty = null;
            string parking = null;
            bool agency = false;
            int numberOfRooms = 0;
            int numberOfBathrooms = 0;
            int size = 0;

            HtmlNodeCollection docs = doc.DocumentNode.SelectNodes("//ul[@class='main-list'] / li");

            foreach (HtmlNode docParameter in docs)
            {
                string nameParam = docParameter.SelectSingleNode("text()").InnerText.Trim();
                string valueParam = docParameter.SelectSingleNode("span / strong")?.InnerText.Trim();

                switch (nameParam)
                {
                    case "Cena":
                        decimal price = ScrapExtensions.PreparePrice(valueParam);
                        break;
                    case "Piętro":
                        int pietro = ScrapExtensions.PrepareNumber(valueParam);
                        break;
                    case "Liczba pokoi":
                        numberOfRooms = ScrapExtensions.PrepareNumber(valueParam);
                        break;
                    case "Powierzchnia":
                        Match result = Regex.Match(valueParam, @"\b[,); +]+.*$");
                        var sizeTemp = valueParam.Replace(result.Value, "");
                        size = ScrapExtensions.PrepareNumber(sizeTemp);
                        break;
                    default:
                        var d = 0;
                        break;
                }
            }

            HtmlNodeCollection subDocs = doc.DocumentNode.SelectNodes("//ul[@class='sub-list'] / li");

            foreach (HtmlNode subDoc in subDocs)
            {
                string nameParam = subDoc.SelectSingleNode("strong").InnerText.Trim().Replace(":", "");
                string valueParam = subDoc.SelectSingleNode("text()")?.InnerText.Trim();

                switch (nameParam)
                {
                    case "Rynek":
                        string rynek = valueParam;
                        break;
                    case "Rodzaj zabudowy":
                        typeOfProperty = valueParam;
                        break;
                    case "Materiał budynku":
                        string materialy = valueParam;
                        break;
                    case "Okna":
                        string onka = valueParam;
                        break;
                    case "Ogrzewanie":
                        string ogrzewanie = valueParam;
                        break;
                    case "Rok budowy":
                        int rokBudowy = ScrapExtensions.PrepareNumber(valueParam);
                        break;
                    case "Stan wykończenia":
                        string stanWykonczenia = valueParam;
                        break;
                    case "Czynsz":
                        decimal czynsz = ScrapExtensions.PreparePrice(valueParam);
                        break;
                    case "Forma własności":
                        string formaWlasnosci = valueParam;
                        break;
                    default:
                        var d = 0;
                        break;
                }
            }

            // location
            var location = doc.DocumentNode.SelectNodes("//address / p[@class='address-links'] / a");
            city = location[1].InnerText.Trim();
            district = location[2].InnerText.Trim();

            // price m2
            decimal tempPriceM2 = (ad.Price / size);
            decimal priceM2 = decimal.Round(tempPriceM2, 2, MidpointRounding.AwayFromZero);

            // user
            var tempUser = doc.DocumentNode.SelectSingleNode(
                "//div[@class='box-person'] / span[@itemprop='name']");
            if (tempUser == null)
            {
                tempUser = doc.DocumentNode.SelectSingleNode(
                    "//div[@class='box-person'] / a / span[@itemprop='name']");
            }
            string username = tempUser?.InnerText?.Trim();
            username = username.Empty() ? "-" : username;

            // agency
            var agencyTemp = doc.DocumentNode.SelectNodes("//h5[@class='box-title']");
            var agent = agencyTemp?.Any(x => x?.InnerText?.Trim() == "Biuro nieruchomości");

            var agencyOfferTemp = doc.DocumentNode.SelectNodes("//h6[@class='box-contact-info-type']");
            bool? priv = agencyOfferTemp?.Any(x => x?.InnerText?.Trim() == "Oferta prywatna");

            agency = !priv.GetValueOrDefault(true) && agent.GetValueOrDefault(true);

            AdDetails adDetails = AdDetails.Create(
                priceM2,
                district,
                city,
                agency,
                typeOfProperty ?? "blok",
                numberOfRooms,
                numberOfBathrooms,
                size,
                username,
                new List<string>(),
                DateTime.UtcNow);

            return adDetails;
        }
    }
}