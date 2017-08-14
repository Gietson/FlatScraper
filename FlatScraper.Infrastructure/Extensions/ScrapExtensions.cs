using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace FlatScraper.Infrastructure.Extensions
{
    public static class ScrapExtensions
    {
        public static async Task<HtmlDocument> ScrapUrl(string url)
        {
            HttpClient hc = new HttpClient();
            HttpResponseMessage result = await hc.GetAsync(url);

            Stream stream = await result.Content.ReadAsStreamAsync();

            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);
            return doc;
        }
    }
}