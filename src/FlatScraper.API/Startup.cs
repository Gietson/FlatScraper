using Autofac;
using Autofac.Extensions.DependencyInjection;
using FlatScraper.Common.Logging;
using FlatScraper.Infrastructure.IoC;
using FlatScraper.Infrastructure.Mongo;
using FlatScraper.Infrastructure.Services;
using FlatScraper.Infrastructure.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace FlatScraper.API
{
    public class Startup
	{
		public IConfigurationRoot Configuration { get; set; }
		public IContainer ApplicationContainer { get; private set; }

		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSerilog(Configuration);
			services.AddCors();
			services.AddMvc()
				.AddJsonOptions(opts => { opts.SerializerSettings.Formatting = Formatting.Indented; });

			var builder = new ContainerBuilder();
			builder.Populate(services);
			builder.RegisterModule(new ContainerModule(Configuration));
			ApplicationContainer = builder.Build();

			return new AutofacServiceProvider(ApplicationContainer);
		}


		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env,
			ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
		{
			app.UseSerilog(loggerFactory);
			app.UseCors(builder => builder.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowAnyOrigin()
				.AllowCredentials());
			MongoConfigurator.Initialize();

			var generalSettings = app.ApplicationServices.GetService<GeneralSettings>();
			if (generalSettings.SeedData)
			{
				var dataInitializer = app.ApplicationServices.GetService<IDataInitializer>();
				dataInitializer.SeedAsync();
			}

			app.UseExceptionHandler();
			app.UseMvc();
			appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
		}
	}
}