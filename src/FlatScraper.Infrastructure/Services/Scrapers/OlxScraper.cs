using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using FlatScraper.Core.Domain;
using FlatScraper.Infrastructure.Extensions;
using HtmlAgilityPack;
using Serilog;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
	public class OlxScraper : IScraper
	{
		private static readonly ILogger Logger = Log.Logger;

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

			HtmlNode details = doc.DocumentNode.SelectSingleNode(
				"//div[@class='offer-titlebox'] / div[@class='offer-titlebox__details']");

			if (details == null)
			{
				Logger.Error("Docs is null. Perhaps url is Otodom: {@ad}", ad);
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
				DateTime.ParseExact(regexAfterChar, "HH:mm, d MMMM yyyy", CultureInfo.CreateSpecificCulture("pl-PL"));

			var offerDescriptions = doc.DocumentNode.SelectNodes(
				"//div[@id='offerdescription'] / div[contains(@class, 'descriptioncontent')] / table / tr / td");


			foreach (var description in offerDescriptions)
			{
				var name = description.SelectSingleNode("table / tr / th")?.InnerText.Trim();
				var value = description.SelectSingleNode("table / tr / td / strong")?.InnerText?.Trim();

				switch (name)
				{
					case "Oferta od":
						if (value == "Osoby prywatnej")
							agency = false;
						else if (value == "Biuro / Deweloper")
							agency = true;
						else
							agency = true;
						break;
					case "Cena za m2":
						priceM2 = ScrapExtensions.ConvertStringToDecimal(value);
						break;
					case "Poziom":
						int poziom = ScrapExtensions.ConvertStringToInt(value);
						break;
					case "Umeblowane":
						/*bool umeblowanie = false;
						if (value == "Tak")
						    umeblowanie = true;
						else if (value == "Nie")
						    umeblowanie = false;
						else
						    umeblowanie = false;*/
						break;
					case "Rynek":
						string rynek = value;
						break;
					case "Rodzaj zabudowy":
						typeOfProperty = value;
						break;
					case "Powierzchnia":
						size = ScrapExtensions.ConvertStringToFloat(value.Replace("m2", ""));
						break;
					case "Liczba pokoi":
						numberOfRooms = ScrapExtensions.ConvertStringToInt(value);
						break;
					case "Finanse":
						break;
					default:
						break;
				}
			}

			var tempUsername = doc.DocumentNode.SelectSingleNode("//div[@class='offer-user__details'] / h4 / a");
			string username = tempUsername?.InnerText?.Trim();

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