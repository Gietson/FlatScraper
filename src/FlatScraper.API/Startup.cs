using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

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
            var serilogOptions = new SerilogOptions();
            Configuration.GetSection("serilog").Bind(serilogOptions);
            services.AddSingleton<SerilogOptions>(serilogOptions);
            services.AddLogging();

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
            loggerFactory.AddSerilog();
            var serilogOptions = app.ApplicationServices.GetService<SerilogOptions>();
            var level = (LogEventLevel) Enum.Parse(typeof(LogEventLevel), serilogOptions.Level, true);
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Is(level)
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(serilogOptions.ApiUrl))
                    {
                        MinimumLogEventLevel = level,
                        AutoRegisterTemplate = true,
                        IndexFormat = string.IsNullOrWhiteSpace(serilogOptions.IndexFormat)
                            ? "logstash-{0:yyyy.MM.dd}"
                            : serilogOptions.IndexFormat
                        /*ModifyConnectionSettings = x =>
                            serilogOptions.UseBasicAuth ?
                                x.BasicAuthentication(serilogOptions.Username, serilogOptions.Password) :
                                x*/
                    })
                .CreateLogger();

            app.UseDeveloperExceptionPage();

            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }*/

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

        private void ConfigureSerilog(IHostingEnvironment env)
        {
            var configuration = new LoggerConfiguration();

            if (env.IsProduction())
            {
                configuration = configuration.MinimumLevel.Warning();
            }
            else
            {
                configuration = configuration.MinimumLevel.Information();
            }

            Log.Logger = configuration
                .Enrich.FromLogContext()
                .WriteTo.File("../logs.txt")
                .CreateLogger();
        }
    }
}