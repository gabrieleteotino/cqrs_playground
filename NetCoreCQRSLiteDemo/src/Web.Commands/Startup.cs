using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Domain.Commands;
using FluentValidation.AspNetCore;

namespace Web.Commands
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new Filters.ValidationActionFilter());
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            // Add application services
            // CQRS
            services.AddSingleton<CQRSlite.Bus.InProcessBus>(new CQRSlite.Bus.InProcessBus());
            services.AddSingleton<CQRSlite.Commands.ICommandSender>(y => y.GetService<CQRSlite.Bus.InProcessBus>());
            services.AddSingleton<CQRSlite.Events.IEventPublisher>(y => y.GetService<CQRSlite.Bus.InProcessBus>());
            services.AddSingleton<CQRSlite.Bus.IHandlerRegistrar>(y => y.GetService<CQRSlite.Bus.InProcessBus>());
            services.AddScoped<CQRSlite.Domain.ISession, CQRSlite.Domain.Session>();
            services.AddSingleton<CQRSlite.Events.IEventStore, Domain.EventStore.InMemoryEventStore>();
            services.AddScoped<CQRSlite.Cache.ICache, CQRSlite.Cache.MemoryCache>();
            services.AddScoped<CQRSlite.Domain.IRepository>(y =>
                new CQRSlite.Cache.CacheRepository(
                    new CQRSlite.Domain.Repository(
                        y.GetService<CQRSlite.Events.IEventStore>()),
                        y.GetService<CQRSlite.Events.IEventStore>(),
                        y.GetService<CQRSlite.Cache.ICache>()));

            // Repositories
            services.AddScoped<Domain.ReadModel.Repositories.IEmployeeRepository, Domain.ReadModel.Repositories.EmployeeRepository>();
            services.AddScoped<Domain.ReadModel.Repositories.ILocationRepository, Domain.ReadModel.Repositories.LocationRepository>();

            //TODO terminate event and command handlers registration
            //https://github.com/gautema/CQRSlite/blob/master/Sample/CQRSWeb/Startup.cs

            // AutoMapper
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles.EmployeeProfile>();

            });

            // Redis
            services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(provider => StackExchange.Redis.ConnectionMultiplexer.Connect("localhost"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
