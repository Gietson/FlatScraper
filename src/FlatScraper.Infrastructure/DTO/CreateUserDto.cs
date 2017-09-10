namespace FlatScraper.Infrastructure.DTO
{
	public class CreateUserDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Username { get; set; }
		public string Role { get; set; }
	}
}