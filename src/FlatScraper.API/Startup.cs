﻿using Autofac;
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
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	            .AddJwtBearer(cfg =>
	            {
	                cfg.RequireHttpsMetadata = false;
	                cfg.SaveToken = true;
	                cfg.TokenValidationParameters = new TokenValidationParameters()
	                {
	                    ValidIssuer = Configuration["jwt:issuer"],
	                    ValidAudience = Configuration["jwt:validateIssuer"],
	                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"]))
	                };
	            });

	        services.AddAuthorization(x => x.AddPolicy("admin", p => p.RequireRole("admin")));
	        services.AddSerilog(Configuration);

	        services.AddMvc()
	            .AddJsonOptions(opts => { opts.SerializerSettings.Formatting = Formatting.Indented; });

	        services.AddWebEncoders();
	        services.AddCors();

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
		    app.UseAuthentication();

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