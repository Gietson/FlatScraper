using System;

namespace FlatScraper.Core.Domain
{
    public class Ad
    {
        public Guid Id { get; protected set; }
        public string IdAds { get; set; }
        public string Title { get; protected set; }
        public string Url { get; protected set; }
        public Decimal Price { get; protected set; }
        public string Page { get; protected set; }

        public DateTime CreateAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Ad() { }

        public Ad(Guid id, string title, string url, decimal price, string page)
        {
            Id = id;
            SetTitle(title);
            SetUrl(url);
            SetPrice(price);
            SetPage(page);
            CreateAt = DateTime.UtcNow;
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

            Url = url.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPrice(decimal price)
        {
            if (price < 0 && price > 100000000)
            { 
                throw new ArgumentOutOfRangeException("Price should be bettwen 0 - 100 000 000");
            }

            Price = price;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPage(string page)
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
    }
}
