using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FlatScraper.Infrastructure.Services.Scrapers;
using HtmlAgilityPack;

namespace FlatScraper.Infrastructure.Extensions
{
    public static class ScrapExtensions
    {
        public static HtmlDocument ScrapUrl(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(url);
            return htmlDoc;
        }

        public static decimal PreparePrice(string price)
        {
            if (string.IsNullOrEmpty(price))
                return 0;

            Regex digitsOnly = new Regex(@"[^\d]");
            string p = digitsOnly.Replace(price, "");

            return decimal.TryParse(p, out decimal tempPrice) ? tempPrice : 0;
        }

        public static IEnumerable<Type> GetScraperTypes()
        {
            Type type = typeof(IScraper);
            Type[] assembly = Assembly.GetAssembly(type).GetTypes();

            IEnumerable<Type> scraperTypes = assembly.Where(x =>
                x.GetInterfaces().Contains(typeof(IScraper)) && x.GetConstructor(Type.EmptyTypes) != null);

            return scraperTypes;
        }
    }
}