using System.ComponentModel.DataAnnotations;

namespace FlatScraper.Infrastructure.DTO
{
    public class LoginUserDto
    {
        [Required]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
