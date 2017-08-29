using Autofac;
using FlatScraper.Infrastructure.Extensions;
using FlatScraper.Infrastructure.Mongo;
using FlatScraper.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;

namespace FlatScraper.Infrastructure.IoC.Modules
{
    public class SettingsModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public SettingsModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration.GetSettings<GeneralSettings>())
                .SingleInstance();
            builder.RegisterInstance(_configuration.GetSettings<MongoSettings>())
                .SingleInstance();
        }
    }
}