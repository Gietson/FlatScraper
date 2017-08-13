using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatScraper.Infrastructure.DTO
{
    public class AdDto
    {
        public Guid Id { get; set; }
        public string IdAds { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Decimal Price { get; set; }
        public string Page { get; set; }
    }
}
