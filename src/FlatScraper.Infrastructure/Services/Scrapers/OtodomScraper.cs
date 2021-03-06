﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FlatScraper.Common.Extensions;
using FlatScraper.Core.Domain;
using FlatScraper.Infrastructure.DTO;
using FlatScraper.Infrastructure.Extensions;
using HtmlAgilityPack;
using Serilog;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
    public class OtodomScraper : IScraper
    {
        private static readonly ILogger Logger = Log.Logger;

        public List<Ad> ParseHomePage(HtmlDocument doc, ScanPageDto scanPage)
        {
            List<Ad> adsList = new List<Ad>();
            HtmlNodeCollection docs = doc.DocumentNode.SelectNodes("//div[@class='row'] / div / article");
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

                decimal price = ScrapExtensions.ConvertStringToDecimal(priceTemp);

                Ad ads = Ad.Create(Guid.NewGuid(), idAds, title, url, price, scanPage.Host, scanPage.HostUrl);

                adsList.Add(ads);
            }

            return adsList;
        }

        public AdDetails ParseDetailsPage(HtmlDocument doc, Ad ad)
        {
            try
            {
                DateTime createAt = DateTime.UtcNow;
                string district = null;
                string city = null;
                string typeOfProperty = null;
                //string parking = null;
                bool agency = false;
                int numberOfRooms = 0;
                int numberOfBathrooms = 0;
                float size = 0;
                decimal priceM2 = 0;
                List<string> images = new List<string>();

                HtmlNodeCollection docs = doc.DocumentNode.SelectNodes("//ul[@class='main-list'] / li");
                if (docs == null)
                {
                    Logger.Error("Docs is null. Perhaps problem with scrap url: {@ad}", ad);
                    return null;
                }

                // images
                var imagesTemp = doc.DocumentNode.SelectNodes("//figure[@itemprop='associatedMedia'] / a / img");

                foreach (var img in imagesTemp)
                {
                    string res = img?.Attributes["src"]?.Value;
                    images.Add(res);
                }

                foreach (HtmlNode docParameter in docs)
                {
                    string nameParam = docParameter.SelectSingleNode("text()").InnerText.Trim();
                    string valueParam = docParameter.SelectSingleNode("span / strong")?.InnerText.Trim();

                    switch (nameParam)
                    {
                        case "Cena":
                            decimal price = ScrapExtensions.ConvertStringToDecimal(valueParam);
                            break;
                        case "Piętro":
                            int pietro = ScrapExtensions.ConvertStringToInt(valueParam);
                            break;
                        case "Liczba pokoi":
                            numberOfRooms = ScrapExtensions.ConvertStringToInt(valueParam);
                            break;
                        case "Powierzchnia":
                            Match result = Regex.Match(valueParam, @"\b[,); +]+.*$");
                            var sizeTemp = valueParam.Replace(result.Value, "");
                            size = ScrapExtensions.ConvertStringToFloat(sizeTemp);
                            break;
                        default:
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
                            int rokBudowy = ScrapExtensions.ConvertStringToInt(valueParam);
                            break;
                        case "Stan wykończenia":
                            string stanWykonczenia = valueParam;
                            break;
                        case "Czynsz":
                            decimal czynsz = ScrapExtensions.ConvertStringToDecimal(valueParam);
                            break;
                        case "Forma własności":
                            string formaWlasnosci = valueParam;
                            break;
                        default:
                            break;
                    }
                }

                // location
                var location = doc.DocumentNode.SelectNodes("//address / p[@class='address-links'] / a");
                city = location[1].InnerText.Trim();
                district = location.Count < 3 ? "-" : location[2].InnerText?.Trim();

                // price m2
                if (size != 0)
                {
                    decimal tempPriceM2 = (ad.Price / (decimal) size);
                    priceM2 = decimal.Round(tempPriceM2, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    priceM2 = 0;
                }

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

                agency = !priv.GetValueOrDefault(false) && agent.GetValueOrDefault(true);


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
                    images,
                    DateTime.UtcNow);

                return adDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}