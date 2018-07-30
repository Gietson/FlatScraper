using Microsoft.Extensions.Configuration;

namespace FlatScraper.Common.Extensions
{
    public static class OptionsExtensions
    {
        internal static TModel GetOptions<TModel>(this IConfiguration configuration, string sectionName)
            where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(sectionName).Bind(model);
            return model;
        }
    }
}