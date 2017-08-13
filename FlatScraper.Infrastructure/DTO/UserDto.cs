using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatScraper.Infrastructure.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
    }
}
