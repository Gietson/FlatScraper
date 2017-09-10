using System;

namespace FlatScraper.Infrastructure.DTO
{
	public class ScanPageDto
	{
		public Guid Id { get; set; }
		public string UrlAddress { get; set; }
		public string Page { get; set; }
		public bool Active { get; set; }
	}
}