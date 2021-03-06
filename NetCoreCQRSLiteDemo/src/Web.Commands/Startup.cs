﻿using System;
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
using AutoMapper;

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
            // CQRS Bus registration
            var bus = new CQRSlite.Bus.InProcessBus();
            services.AddSingleton<CQRSlite.Commands.ICommandSender>(bus);
            services.AddSingleton<CQRSlite.Events.IEventPublisher>(bus);
            services.AddSingleton<CQRSlite.Bus.IHandlerRegistrar>(bus);

            services.AddScoped<CQRSlite.Domain.ISession, CQRSlite.Domain.Session>();
            services.AddSingleton<CQRSlite.Events.IEventStore, Domain.EventStore.InMemoryEventStore>();
            services.AddScoped<CQRSlite.Cache.ICache, CQRSlite.Cache.MemoryCache>();
            //services.AddScoped<CQRSlite.Domain.IRepository>(y =>
            //    new CQRSlite.Cache.CacheRepository(
            //        new CQRSlite.Domain.Repository(
            //            y.GetService<CQRSlite.Events.IEventStore>()),
            //            y.GetService<CQRSlite.Events.IEventStore>(),
            //            y.GetService<CQRSlite.Cache.ICache>()));
            services.AddScoped<CQRSlite.Domain.IRepository>(y => new CQRSlite.Domain.Repository(y.GetService<CQRSlite.Events.IEventStore>()));

            // Repositories
            services.AddScoped<Domain.ReadModel.Repositories.IEmployeeRepository, Domain.ReadModel.Repositories.EmployeeRepository>();
            services.AddScoped<Domain.ReadModel.Repositories.ILocationRepository, Domain.ReadModel.Repositories.LocationRepository>();

            //TODO terminate event and command handlers registration
            //https://github.com/gautema/CQRSlite/blob/master/Sample/CQRSWeb/Startup.cs

            // Manual registration
            // https://github.com/luisrudge/dotnetfloripa-es/blob/master/app/api/Startup.cs
            // Command Handlers
            services.AddTransient<CQRSlite.Commands.ICommandHandler<CreateEmployeeCommand>, Domain.CommandHandlers.EmployeeCommandHandler>();
            services.AddTransient<Domain.CommandHandlers.EmployeeCommandHandler>();

            services.AddTransient<CQRSlite.Commands.ICommandHandler<CreateLocationCommand>, Domain.CommandHandlers.LocationCommandHandler>();
            services.AddTransient<CQRSlite.Commands.ICommandHandler<AssignEmployeeToLocationCommand>, Domain.CommandHandlers.LocationCommandHandler>();
            services.AddTransient<CQRSlite.Commands.ICommandHandler<RemoveEmployeeFromLocationCommand>, Domain.CommandHandlers.LocationCommandHandler>();
            services.AddTransient<Domain.CommandHandlers.LocationCommandHandler>();

            // Event Handlers
            services.AddTransient<CQRSlite.Events.IEventHandler<Domain.Events.EmployeeCreatedEvent>, Domain.EventHandlers.EmployeeEventHandler>();
            services.AddTransient<Domain.EventHandlers.EmployeeEventHandler>();

            services.AddTransient<CQRSlite.Events.IEventHandler<Domain.Events.LocationCreatedEvent>, Domain.EventHandlers.LocationEventHandler>();
            services.AddTransient<CQRSlite.Events.IEventHandler<Domain.Events.EmployeeAssignedToLocationEvent>, Domain.EventHandlers.LocationEventHandler>();
            services.AddTransient<CQRSlite.Events.IEventHandler<Domain.Events.EmployeeRemovedFromLocationEvent>, Domain.EventHandlers.LocationEventHandler>();
            services.AddTransient<Domain.EventHandlers.LocationEventHandler>();

            // AutoMapper
            services.AddAutoMapper();

            // Redis
            services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(provider => StackExchange.Redis.ConnectionMultiplexer.Connect("localhost"));

            // Register command handlers in CQRSLite
            var serviceProvider = services.BuildServiceProvider();
            var registrar = new CQRSlite.Config.BusRegistrar(new DependencyResolver(serviceProvider));
            // Only one type for assembly is required
            registrar.Register(typeof(Domain.CommandHandlers.LocationCommandHandler));
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
