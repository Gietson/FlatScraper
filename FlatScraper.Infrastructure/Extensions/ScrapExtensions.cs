using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FlatScraper.Infrastructure.Extensions
{
    public static class ScrapExtensions
    {
        public async static Task<HtmlDocument> ScrapUrl(string url)
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
