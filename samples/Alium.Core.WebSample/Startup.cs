using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Alium.Core.WebSample
{
    using System.Threading;
    using Alium.DependencyInjection;
    using Alium.Modules;
    using Alium.Tasks;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IModuleProvider moduleProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            foreach (var module in moduleProvider.Modules)
            {
                Console.WriteLine($"Module: {module.Id}\n");
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }

    public class AppModule : ModuleBase, IServicesBuilder
    {
        public AppModule()
            : base(new ModuleId("AppModule"), "AppModule", "Application module", new[] { CoreInfo.CoreModuleId }) { }

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

    public class AppModuleStartupTask : IStartupTask
    {
        private readonly ILogger _logger;

        public AppModuleStartupTask(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AppModuleStartupTask>();
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("In startup task");

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

        public Task ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("In shutdown task");

            return Task.CompletedTask;
        }
    }
}
