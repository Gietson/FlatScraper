﻿using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace FlatScraper.Infrastructure.Extensions
{
    public static class ScrapExtensions
    {
        public static async Task<HtmlDocument> ScrapUrl(string url)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(url);

                /*HttpClient hc = new HttpClient();
                HttpResponseMessage result = await hc.GetAsync(url);

                Stream stream = await result.Content.ReadAsStreamAsync();

                HtmlDocument doc = new HtmlDocument();
                doc.Load(stream);*/
                return htmlDoc;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static decimal PreparePrice(string price)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            string p = digitsOnly.Replace(price, "");

            return decimal.TryParse(p, out decimal tempPrice) ? tempPrice : 0;
        }
    }
}