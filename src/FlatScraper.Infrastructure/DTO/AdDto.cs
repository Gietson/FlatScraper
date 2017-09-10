using System;

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
		public AdDetailsDto AdDetails { get; set; }
	}
}