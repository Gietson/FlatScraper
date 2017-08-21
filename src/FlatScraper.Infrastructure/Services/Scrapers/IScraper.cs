﻿using System.Collections.Generic;
using FlatScraper.Core.Domain;
using HtmlAgilityPack;

namespace FlatScraper.Infrastructure.Services.Scrapers
{
    public interface IScraper : IService
    {
        List<Ad> ParseHomePage(HtmlDocument doc);
        AdDetails ParseDetailsPage(HtmlDocument doc);
    }
}