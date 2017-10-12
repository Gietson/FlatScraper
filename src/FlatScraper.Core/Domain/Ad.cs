using System;

namespace FlatScraper.Core.Domain
{
	public class Ad
	{
		public Guid Id { get; protected set; }
		public string IdAds { get; set; }
		public string Title { get; protected set; }
		public string Url { get; protected set; }
		public decimal Price { get; protected set; }
		public string Host { get; protected set; }
	    public string HostUrl { get; protected set; }

		public AdDetails AdDetails { get; set; }

		public DateTime CreateAt { get; protected set; }
		public DateTime UpdatedAt { get; protected set; }

		protected Ad()
		{
		}

		protected Ad(Guid id, string idAds, string title, string url, decimal price, string host, string hostUrl)
		{
			Id = id;
			SetIdAds(idAds);
			SetTitle(title);
			SetUrl(url);
			SetPrice(price);
			SetHost(host);
		    SetHostUrl(hostUrl);
            CreateAt = DateTime.UtcNow;
		}

		private void SetIdAds(string idAds)
		{
			if (string.IsNullOrWhiteSpace(idAds))
			{
				throw new ArgumentNullException("Id Ads can not be empty.");
			}
			if (IdAds == idAds)
			{
				return;
			}

			IdAds = idAds;
			UpdatedAt = DateTime.UtcNow;
		}

		public void SetUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentNullException("Email can not be empty.");
			}
			if (Url == url)
			{
				return;
			}

			Url = url;
			UpdatedAt = DateTime.UtcNow;
		}

		public void SetPrice(decimal price)
		{
			if (price < 0 && price > 100000000)
			{
				throw new ArgumentOutOfRangeException("Price should be between 0 - 100 000 000");
			}

			Price = price;
			UpdatedAt = DateTime.UtcNow;
		}

		public void SetHost(string host)
		{
			if (string.IsNullOrWhiteSpace(host))
			{
				throw new ArgumentNullException("Host can not be empty.");
			}
			if (Host == host)
			{
				return;
			}

			Host = host.ToLowerInvariant();
			UpdatedAt = DateTime.UtcNow;
		}

	    public void SetHostUrl(string hostUrl)
	    {
	        if (string.IsNullOrWhiteSpace(hostUrl))
	        {
	            throw new ArgumentNullException("HostUrl can not be empty.");
	        }
	        if (HostUrl == hostUrl)
	        {
	            return;
	        }

	        HostUrl = hostUrl.ToLowerInvariant();
	        UpdatedAt = DateTime.UtcNow;
	    }

        public void SetTitle(string title)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentNullException("Email can not be empty.");
			}
			if (Title == title)
			{
				return;
			}

			Title = title;
			UpdatedAt = DateTime.UtcNow;
		}

		public static Ad Create(Guid id, string idAds, string title, string url, decimal price, string host, string hostUrl)
			=> new Ad(id, idAds, title, url, price, host, hostUrl);
	}
}