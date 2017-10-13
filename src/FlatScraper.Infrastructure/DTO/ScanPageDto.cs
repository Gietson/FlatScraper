using System;

namespace FlatScraper.Infrastructure.DTO
{
    public class ScanPageDto
    {
        public Guid Id { get; set; }
        public string UrlAddress { get; set; }
        public string Host { get; set; }
        public string HostUrl { get; set; }
        public bool Active { get; set; }
    }
}