// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyModel;
    using Microsoft.Extensions.Hosting;

    using Alium.Infrastructure;

    /// <summary>
    /// Provides extensions for the <see cref="IHostBuilder"/> type.
    /// </summary>
    public static class ModuleHostBuilderExtensions
    {
        /// <summary>
        /// Configures the application to use a specific set of modules.
        /// </summary>
        /// <param name="builder">The host builder.</param>
        /// <param name="modules">The set of modules.</param>
        /// <returns>The host builder.</returns>
        public static IHostBuilder UseModules(this IHostBuilder builder, params IModule[] modules)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNull(modules, nameof(modules));
            
            // MA - Create the framework initialiser
            var init = FrameworkInitialiser.FromModules(modules, CreateFrameworkConfiguration());

            builder.ConfigureAppConfiguration((ctx, bldr) =>
            {
                // MA - Extend the configuration with elements from module and features.
                init.ExtendConfiguration(ctx, bldr);
            });

            builder.ConfigureServices(services =>
            {
                // MA Add any module and feature services.
                init.AddServices(services);
            });

            return builder;
        }

        /// <summary>
        /// Configures the application to discover modules provided by a dependency context.
        /// If no dependency context is specified, the application will use the dependency context of the entry assembly.
        /// </summary>
        /// <param name="builder">The host builder.</param>
        /// <param name="dependencyContext">[Optional] The dependency context.</param>
        /// <returns>The host builder.</returns>
        public static IHostBuilder UseDiscoveredModules(this IHostBuilder builder, DependencyContext? dependencyContext = null)
        {
            Ensure.IsNotNull(builder, nameof(builder));

            dependencyContext = dependencyContext ?? DependencyContext.Default;
            if (dependencyContext != null)
            {
                // MA - Create the framework initialiser
                var init = FrameworkInitialiser.FromDependencyContext(dependencyContext, CreateFrameworkConfiguration());

                builder.ConfigureAppConfiguration((ctx, bldr) =>
                {
                    // MA - Extend the configuration with elements from module and features.
                    init.ExtendConfiguration(ctx, bldr);
                });

                // MA - Add our module provider and part manager to the application services.
                builder.ConfigureServices(services =>
                {
                    // MA Add any module and feature services.
                    init.AddServices(services);
                });
            }

            return builder;
        }

        private static IConfiguration CreateFrameworkConfiguration()
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile(CoreInfo.FeaturesConfigurationFile, optional: true);

            return builder.Build();
        }
    }
}
