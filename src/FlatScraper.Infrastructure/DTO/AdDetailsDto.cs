using System;
using System.Collections.Generic;

namespace FlatScraper.Infrastructure.DTO
{
	public class AdDetailsDto
	{
		private IEnumerable<string> Photos { get; set; }

		public decimal PriceM2 { get; protected set; }
		public string District { get; protected set; }
		public string City { get; protected set; }
		public bool Agency { get; protected set; }
		public string PropertyType { get; protected set; }
		public int NumberOfRooms { get; protected set; }
		public int NumberOfBathrooms { get; protected set; }
		public float Size { get; protected set; }
		public string UserName { get; protected set; }

		public DateTime UpdatedAt { get; protected set; }
		public DateTime CreateAt { get; protected set; }
	}
}