using System;
using System.Collections.Generic;

namespace FlatScraper.Core.Domain
{
    public class AdDetails
    {
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

        public List<string> Photos { get; protected set; }

        protected AdDetails()
        {
        }

        protected AdDetails(decimal priceM2, string district, string city, bool agency, string propertyType,
            int numberOfRooms, int numberOfBathrooms, float size, string userName, List<string> photos,
            DateTime createAt)
        {
            SetPriceM2(priceM2);
            SetDistrict(district);
            SetCity(city);
            SetAgency(agency);
            SetPropertyType(propertyType);
            SetNumberOfRooms(numberOfRooms);
            SetNumberOfBathrooms(numberOfBathrooms);
            SetSize(size);
            SetUserName(userName);
            SetPhotos(photos);
            SetCreateAt(createAt);
        }

        private void SetCreateAt(DateTime createAt)
        {
            CreateAt = createAt;
        }


        private void SetPhotos(List<string> photos)
        {
            Photos = photos;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetUserName(string userName)
        {
            if (UserName == userName)
            {
                return;
            }

            UserName = userName;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetSize(float size)
        {
            if (float.IsNaN(size))
            {
                throw new Exception("Size must be a number");
            }

            Size = size;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetNumberOfBathrooms(int numberOfBathrooms)
        {
            if (numberOfBathrooms < 0)
            {
                throw new Exception("NumberOfBathrooms must be greater than 0.");
            }
            if (NumberOfBathrooms == numberOfBathrooms)
            {
                return;
            }

            NumberOfBathrooms = numberOfBathrooms;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetNumberOfRooms(int numberOfRooms)
        {
            if (numberOfRooms < 0)
            {
                throw new Exception("NumberOfRooms must be greater than 0.");
            }
            if (NumberOfRooms == numberOfRooms)
            {
                return;
            }

            NumberOfRooms = numberOfRooms;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetPropertyType(string propertyType)
        {
            PropertyType = propertyType;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetAgency(bool agency)
        {
            Agency = agency;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentNullException("City can not be empty.");
            }
            if (City == city)
            {
                return;
            }

            City = city;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetPriceM2(decimal priceM2)
        {
            if (priceM2 < 0)
            {
                throw new Exception("Price/M2 must be greater than 0.");
            }
            if (PriceM2 == priceM2)
            {
                return;
            }

            PriceM2 = priceM2;
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetDistrict(string district)
        {
            if (string.IsNullOrWhiteSpace(district))
            {
                throw new ArgumentNullException("District can not be empty.");
            }
            if (District == district)
            {
                return;
            }

            District = district;
            UpdatedAt = DateTime.UtcNow;
        }

        public static AdDetails Create(decimal priceM2, string district, string city, bool agency, string propertyType,
            int numberOfRooms, int numberOfBathrooms, float size, string userName, List<string> photos,
            DateTime createAt)
            => new AdDetails(priceM2, district, city, agency, propertyType, numberOfRooms, numberOfBathrooms, size,
                userName, photos, createAt);
    }
}