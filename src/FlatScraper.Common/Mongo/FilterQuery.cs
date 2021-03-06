﻿namespace FlatScraper.Common.Mongo
{
    public class FilterQuery
    {
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public decimal PriceM2From { get; set; }
        public decimal PriceM2To { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int SizeFrom { get; set; }
        public int SizeTo { get; set; }
        public bool? Agency { get; set; }
    }
}