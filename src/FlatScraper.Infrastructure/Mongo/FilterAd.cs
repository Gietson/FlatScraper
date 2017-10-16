using FlatScraper.Common.Mongo;
using FlatScraper.Core.Domain;
using MongoDB.Driver.Linq;

namespace FlatScraper.Infrastructure.Mongo
{
    public static class FilterAd
    {
        public static IMongoQueryable<Ad> FilterAds(this IMongoQueryable<Ad> collection,
            PagedQueryBase query)
            => collection.FilterAds(query.Filter);

        public static IMongoQueryable<Ad> FilterAds(this IMongoQueryable<Ad> collection, FilterQuery filter)
        {
            if (!string.IsNullOrEmpty(filter.City))
            {
                collection = collection.Where(x =>
                    x.AdDetails.City.ToLower().Trim().Contains(filter.City.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(filter.District))
            {
                collection = collection.Where(x =>
                    x.AdDetails.District.ToLower().Trim().Contains(filter.District.ToLower().Trim()));
            }
            if (filter.PriceFrom > 0)
            {
                collection = collection.Where(x => x.Price >= filter.PriceFrom);
            }
            if (filter.PriceTo > 0)
            {
                collection = collection.Where(x => x.Price <= filter.PriceTo);
            }
            if (filter.SizeFrom > 0)
            {
                collection = collection.Where(x => x.AdDetails.Size >= filter.SizeFrom);
            }
            if (filter.SizeTo > 0)
            {
                collection = collection.Where(x => x.AdDetails.Size <= filter.SizeTo);
            }
            if (filter.Agency != null)
            {
                collection = collection.Where(x => x.AdDetails.Agency == filter.Agency);
            }
            return collection;
        }
    }
}