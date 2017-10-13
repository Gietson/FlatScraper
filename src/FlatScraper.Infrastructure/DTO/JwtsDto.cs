using System;
using System.Collections.Generic;
using System.Text;

namespace FlatScraper.Infrastructure.DTO
{
    public class JwtDto
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
