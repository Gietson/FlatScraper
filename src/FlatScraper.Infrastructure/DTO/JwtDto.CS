using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatScraper.Infrastructure.DTO
{
    public class JwtDto
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
