﻿namespace Alium.Core.WebSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    using Alium.DependencyInjection;
    using Alium.Modules;
    using Alium.Tasks;
    using Alium.Features;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
#pragma warning disable IDE0060 // Remove unused parameter
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IModuleProvider moduleProvider, IFeatureProvider featureProvider, IFeatureStateProvider featureStateProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            foreach (var module in moduleProvider.Modules)
            {
                Console.WriteLine($"Module: {module.Id}");
            }

            foreach (var feature in featureProvider.Features)
            {
                if ((featureStateProvider.GetFeatureState(feature.Id)?.Enabled).GetValueOrDefault(false))
                {
                    Console.WriteLine($"Feature: {feature.Id}");
                }
            }

            app.Run((context) =>
            {
                Console.WriteLine($"Executing for {context.Request.Path}");

                var feature1 = context.RequestServices.GetRequiredService<IFeature<IAppService>>();
                var feature2 = context.RequestServices.GetRequiredService<IFeature<IAppService, AppFeatureConfiguration>>();

                if (feature1.Enabled)
                {
                    feature1.Service?.DoAction();
                }

                if (feature2.Enabled)
                {
                    feature2.Service?.DoAction(feature2.Configuration);
                }

                var tenantService = context.RequestServices.GetRequiredService<ITenantService>();
                tenantService.DoAction();

                return Task.CompletedTask;
            });
        }
    }

    public class AppModule : ModuleBase, IServicesBuilder, IFeaturesBuilder
    {
        public static readonly ModuleId AppModuleId = new ModuleId("AppModule");

        public AppModule()
            : base(new ModuleId("AppModule"), "AppModule", "Application module", new[] { CoreInfo.CoreModuleId }) { }

        public void BuildFeatures(ICollection<IFeature> features)
        {
            features.Add(new AppFeature());
        }

        public void BuildServices(IServiceCollection services)
        {
            services.AddScoped<IStartupTask, AppModuleStartupTask>();
            services.AddScoped<IShutdownTask, AppModuleShutdownTask>();
        }

        public override void Initialise(ModuleInitialisationContext context)
        {
            var logger = context.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger<AppModule>();

            logger.LogInformation("In module initialisation");
        }
    }

    public class AppFeature : FeatureBase, IServicesBuilder
    {
        public AppFeature()
            : base(new FeatureId(AppModule.AppModuleId, "AppFeature"), "AppFeature", "Application feature", enabledByDefault: true) { }

        public void BuildServices(IServiceCollection services)
        {
            services.AddTenantScoped<IAppService, AppService>();

            services.AddScoped<IStartupTask, AppFeatureStartupTask>();
            services.AddScoped<IShutdownTask, AppFeatureShutdownTask>();
            services.AddScoped<ITenantService, TenantService>();
        }

        public override void Initialise(FeatureInitialisationContext context)
        {
            var logger = context.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger<AppFeature>();

            logger.LogInformation("In feature initialisation");
        }
    }

    public class AppFeatureConfiguration
    {
        public string? Message { get; set; }
    }

    public class AppModuleStartupTask : IStartupTask
    {
        private readonly ILogger _logger;

        public AppModuleStartupTask(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AppModuleStartupTask>();
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("In module startup task");

            return Task.CompletedTask;
        }
    }

    public class AppModuleShutdownTask : IShutdownTask
    {
        private readonly ILogger _logger;

        public AppModuleShutdownTask(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AppModuleStartupTask>();
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("In module shutdown task");

            return Task.CompletedTask;
        }
    }

    public class AppFeatureStartupTask : IStartupTask
    {
        private readonly ILogger _logger;

        public AppFeatureStartupTask(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AppFeatureStartupTask>();
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("In feature startup task");

            return Task.CompletedTask;
        }
    }

    public class AppFeatureShutdownTask : IShutdownTask
    {
        private readonly ILogger _logger;

        public AppFeatureShutdownTask(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AppFeatureShutdownTask>();
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("In feature shutdown task");

            return Task.CompletedTask;
        }
    }

    public interface IAppService
    {
        void DoAction();

        void DoAction(AppFeatureConfiguration? config);
    }

    public class AppService : IAppService
    {
        public void DoAction()
        {
            Console.WriteLine("Service action");
        }
        public void DoAction(AppFeatureConfiguration? config)
        {
            Console.WriteLine(config?.Message);
        }
    }

    public interface ITenantService
    {
        void DoAction();
    }

    public class TenantService : ITenantService
    {
        public TenantService(IWorkContext workContext)
        {
            WorkContext = workContext;
        }

        public IWorkContext WorkContext { get; }

        public void DoAction()
        {
            Console.WriteLine($"Hello from {WorkContext.TenantId}");
        }
    }
}
