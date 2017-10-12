using System;
using System.Collections.Generic;
using System.Linq;
using FlatScraper.Common.Extensions;
using FlatScraper.Core.Domain;
using FlatScraper.Infrastructure.Extensions;
using HtmlAgilityPack;
using Serilog;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
	public class GumtreeScraper : IScraper
	{
		private static readonly ILogger Logger = Log.Logger;

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
				decimal price = ScrapExtensions.ConvertStringToDecimal(priceTemp?.InnerText);

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
			//string parking = null;
			bool agency = false;
			int numberOfRooms = 0;
			int numberOfBathrooms = 0;
			float size = 0;
			decimal priceM2 = 0;

			HtmlNodeCollection docs = doc.DocumentNode.SelectNodes("//ul[@class='selMenu'] / li / div");

			foreach (HtmlNode docParameter in docs)
			{
				string nameParam = docParameter.SelectSingleNode("span[@class='name']")?.InnerText.Trim();
				string valueParam = docParameter.SelectSingleNode("span[@class='value']")?.InnerText.Trim();
				if (nameParam.Empty() || valueParam.Empty())
				{
					break;
				}

				switch (nameParam)
				{
					case "Data dodania":
					{
						DateTime.TryParse(valueParam, out DateTime now);
						createAt = now;
					}
						break;
					case "Lokalizacja":
						var location = valueParam?.Split(",");
						if (location != null)
						{
							district = location[0].Trim();
							city = location[1].Trim();
						}
						break;
					case "Na sprzedaż przez":

						if (valueParam == "Właściciel")
						{
							agency = false;
						}
						else if (valueParam == "Agencja")
						{
							agency = true;
						}
						else
						{
							agency = true;
						}
						break;
					case "Rodzaj nieruchomości":
						typeOfProperty = valueParam?.Trim();
						break;
					case "Liczba pokoi":
						numberOfRooms = ScrapExtensions.ConvertStringToInt(valueParam);
						break;
					case "Liczba łazienek":
						numberOfBathrooms = ScrapExtensions.ConvertStringToInt(valueParam);
						break;
					case "Wielkość (m2)":
						size = ScrapExtensions.ConvertStringToFloat(valueParam);
						break;
					case "Parking":
						//parking = valueParam.Trim();
						break;
					default:
						break;
				}
			}
			if (size != 0)
			{
				decimal tempPriceM2 = (ad.Price / (decimal)size);
				priceM2 = decimal.Round(tempPriceM2, 2, MidpointRounding.AwayFromZero);
			}
			else
			{
				priceM2 = 0;
			}

			var tempUsername = doc.DocumentNode.SelectSingleNode("//span[@class='username'] / a /text()");
			string username = tempUsername.InnerText.Trim();


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

			return adDetails;
		}
	}
}