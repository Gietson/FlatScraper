using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FlatScraper.Infrastructure.Services.Scrapers;
using FlatScraper.Common.Extensions;
using HtmlAgilityPack;

namespace FlatScraper.Infrastructure.Extensions
{
	public static class ScrapExtensions
	{
	    private static readonly CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
	    private static readonly NumberStyles style = NumberStyles.Any;

		public static HtmlDocument ScrapUrl(string url)
		{
			HtmlWeb web = new HtmlWeb();
			HtmlDocument htmlDoc = web.Load(url);
			return htmlDoc;
		}

		public static decimal ConvertStringToDecimal(string value)
		{
			if (string.IsNullOrEmpty(value))
				return 0;

		    value = value.Replace(",", ".");
		    Regex digitsOnly = new Regex(@"[^-?\d+\.]");
		    string p = digitsOnly.Replace(value, "");

            decimal result = decimal.TryParse(p, style, culture, out decimal tempPrice) ? tempPrice : 0;

		    return result;
		}

		public static IEnumerable<Type> GetScraperTypes()
		{
			Type type = typeof(IScraper);
			Type[] assembly = Assembly.GetAssembly(type).GetTypes();

			IEnumerable<Type> scraperTypes = assembly.Where(x =>
				x.GetInterfaces().Contains(typeof(IScraper)));

			return scraperTypes;
		}

		public static float ConvertStringToFloat(string value)
		{
			if (value.Empty())
				return 0;

		    value = value.Replace(",", ".");
		    Regex digitsOnly = new Regex(@"[^-?\d+\.]");
		    string p = digitsOnly.Replace(value, "");


            float result = float.TryParse(p, style, culture, out float tempNumber) ? tempNumber : 0;
		    return result;
		}

	    public static int ConvertStringToInt(string value)
	    {
	        if (value.Empty())
	            return 0;

	        decimal resultDecimal = ConvertStringToDecimal(value);
	        int result = (int)Math.Round(resultDecimal, MidpointRounding.AwayFromZero);

            return result;
	    }
	}
}