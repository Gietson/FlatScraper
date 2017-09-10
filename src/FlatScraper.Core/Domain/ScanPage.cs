using System;

namespace FlatScraper.Core.Domain
{
	public class ScanPage
	{
		public Guid Id { get; protected set; }
		public string UrlAddress { get; protected set; }
		public string Page { get; protected set; }
		public DateTime CreateAt { get; protected set; }
		public DateTime UpdatedAt { get; protected set; }
		public bool Active { get; protected set; }

		protected ScanPage()
		{
		}

		protected ScanPage(Guid id, string urlAddress, string page, bool active)
		{
			Id = id;
			SetUrlAddress(urlAddress);
			SetPage(page);
			SetActive(active);
			CreateAt = DateTime.UtcNow;
		}

		private void SetActive(bool active)
		{
			Active = active;
			UpdatedAt = DateTime.UtcNow;
		}

		private void SetUrlAddress(string urlAddress)
		{
			if (string.IsNullOrWhiteSpace(urlAddress))
			{
				throw new ArgumentNullException("UrlAddress can not be empty.");
			}
			if (UrlAddress == urlAddress)
			{
				return;
			}

			UrlAddress = urlAddress.ToLowerInvariant();
			UpdatedAt = DateTime.UtcNow;
		}

		private void SetPage(string page)
		{
			if (string.IsNullOrWhiteSpace(page))
			{
				throw new ArgumentNullException("Page can not be empty.");
			}
			if (Page == page)
			{
				return;
			}

			Page = page.ToLowerInvariant();
			UpdatedAt = DateTime.UtcNow;
		}

		public static ScanPage Create(Guid id, string urlAddress, string page, bool active)
			=> new ScanPage(id, urlAddress, page, active);
	}
}