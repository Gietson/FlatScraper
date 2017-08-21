using Autofac;
using FlatScraper.Infrastructure.IoC.Modules;
using FlatScraper.Infrastructure.Mappers;
using Microsoft.Extensions.Configuration;

namespace FlatScraper.Infrastructure.IoC
{
    public class ContainerModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public ContainerModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(AutoMapperConfig.Initialize());
            builder.RegisterModule<RepositoryModule>();
            builder.RegisterModule<ServiceModule>();

            builder.RegisterModule(new SettingsModule(_configuration));
        }
    }
}