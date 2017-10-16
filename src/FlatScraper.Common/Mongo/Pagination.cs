using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FlatScraper.Common.Mongo
{
    public static class Pagination
    {
        public static async Task<PagedResult<T>> PaginateAsync<T>(this IMongoQueryable<T> collection,
            PagedQueryBase query)
            => await collection.PaginateAsync(query.Filter, query.Page, query.ResultsPerPage);


<<<<<<< HEAD
        public static async Task<PagedResult<T>> PaginateAsync<T>(this IMongoQueryable<T> collection, 
			int page = 1,
            int resultsPerPage = 10)
=======
        public static async Task<PagedResult<T>> PaginateAsync<T>(this IMongoQueryable<T> collection,
            FilterQuery filter, int page = 1, int resultsPerPage = 10)
>>>>>>> 8c7cb3d9055028e201162ce2c1124d1f49627a72
        {
            if (page <= 0)
                page = 1;

            if (resultsPerPage <= 0)
                resultsPerPage = 10;

            var isEmpty = await collection.AnyAsync() == false;
            if (isEmpty)
                return PagedResult<T>.Empty;

            var totalResults = await collection.CountAsync();
            var totalPages = (int) Math.Ceiling((decimal) totalResults / resultsPerPage);
            var data = await collection.Limit(page, resultsPerPage).ToListAsync();

            return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
        }


        public static IMongoQueryable<T> Limit<T>(this IMongoQueryable<T> collection, PagedQueryBase query)
            => collection.Limit(query.Page, query.ResultsPerPage);

        public static IMongoQueryable<T> Limit<T>(this IMongoQueryable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
                page = 1;

            if (resultsPerPage <= 0)
                resultsPerPage = 10;

            var skip = (page - 1) * resultsPerPage;
            var data = collection.Skip(skip)
                .Take(resultsPerPage);

            return data;
        }
    }
}