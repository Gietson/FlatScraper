using System;

namespace FlatScraper.Infrastructure.DTO
{
    public class JwtDto
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
