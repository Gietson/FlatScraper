using System;

namespace FlatScraper.Core.Domain
{
    public class ScanPage
    {
        public Guid Id { get; protected set; }
        public string UrlAddress { get; protected set; }
        public string Host { get; protected set; }
        public string HostUrl { get; protected set; }
        public DateTime CreateAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public bool Active { get; protected set; }

        protected ScanPage()
        {
        }

        protected ScanPage(Guid id, string urlAddress, string host, string hostUrl, bool active)
        {
            Id = id;
            SetUrlAddress(urlAddress);
            SetHost(host);
            SetHostUrl(hostUrl);
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

        private void SetHost(string host)
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

        private void SetHostUrl(string hostUrl)
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

        public static ScanPage Create(Guid id, string urlAddress, string host, string hostUrl, bool active)
            => new ScanPage(id, urlAddress, host, hostUrl, active);
    }
}