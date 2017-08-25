using System.Text.RegularExpressions;
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
    }
}