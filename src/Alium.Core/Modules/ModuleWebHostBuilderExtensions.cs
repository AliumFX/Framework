// Copyright (c) Alium Project. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyModel;

    using Alium.DependencyInjection;
    using Alium.Infrastructure;
    using Alium.Parts;

    /// <summary>
    /// Provides extensions for the <see cref="IWebHostBuilder"/> type.
    /// </summary>
    public static class ModuleWebHostBuilderExtensions
    {
        /// <summary>
        /// Configures the application to use a specific set of modules.
        /// </summary>
        /// <param name="builder">The web host builder.</param>
        /// <param name="modules">The set of modules.</param>
        /// <returns>The web host builder.</returns>
        public static IWebHostBuilder UseModules(this IWebHostBuilder builder, params IModule[] modules)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNull(modules, nameof(modules));

            // MA - Create the module provider.
            var provider = new ModuleProvider(modules);

            // MA - Create the part manager.
            var partManager = new PartManager();
            foreach (var assembly in provider.Modules.Select(m => m.GetType().Assembly).Distinct())
            {
                // MA - Add the module assemblies as parts.
                partManager.Parts.Add(new AssemblyPart(assembly));
            }

            // MA - Add the module part feature provider.
            partManager.PartFeatureProviders.Add(new ModulePartFeatureProvider());

            // MA - Add our module provider and part manager to the application services.
            builder.ConfigureServices(services =>
            {
                // Add the module provider and part manager.
                EnsureModuleProviderAndPartManager(services, provider, partManager);

                // Add any module services.
                services.AddModuleServices(provider);
            });

            return builder;
        }

        /// <summary>
        /// Configures the application to discover modules provided by a dependency context.
        /// If no dependency context is specified, the application will use the dependency context of the entry assembly.
        /// </summary>
        /// <param name="builder">The web host builder.</param>
        /// <param name="dependencyContext">[Optional] The dependency context.</param>
        /// <returns>The web host builder.</returns>
        public static IWebHostBuilder UseDiscoveredModules(this IWebHostBuilder builder, DependencyContext dependencyContext = null)
        {
            Ensure.IsNotNull(builder, nameof(builder));

            dependencyContext = dependencyContext ?? DependencyContext.Default;
            if (dependencyContext != null)
            {
                // MA - Create the assembly provider
                var assemblyProvider = new DependencyContextAssemblyProvider(dependencyContext);

                // MA - Create the part manager.
                var partManager = new PartManager();
                foreach (var assembly in assemblyProvider.Assemblies)
                {
                    // MA - Add the discovered parts.
                    partManager.Parts.Add(new AssemblyPart(assembly));
                }

                // MA - Add the module part feature provider.
                partManager.PartFeatureProviders.Add(new ModulePartFeatureProvider());

                // MA - Create the module part feature and populate it.
                var feature = new ModulePartFeature();
                partManager.PopulateFeature(feature);

                // MA - Enumerate available modules add create a module provider.
                var modules = feature.ModuleTypes.Select(t => (IModule)Activator.CreateInstance(t));
                var provider = new ModuleProvider(modules);

                // MA - Add our module provider and part manager to the application services.
                builder.ConfigureServices(services =>
                {
                    // Add the module provider and part manager.
                    EnsureModuleProviderAndPartManager(services, provider, partManager);

                    // Add any module services.
                    services.AddModuleServices(provider);
                });
            }

            return builder;
        }

        private static void EnsureModuleProviderAndPartManager(
            IServiceCollection services, 
            IModuleProvider moduleProvider,
            IPartManager partManager)
        {
            var providerDescriptor = services.SingleOrDefault(sd => sd.ServiceType == typeof(IModuleProvider));
            if (providerDescriptor != null)
            {
                services.Remove(providerDescriptor);
            }
            services.Add(ServiceDescriptor.Singleton<IModuleProvider>(sp => moduleProvider));

            var managerDescriptor = services.SingleOrDefault(sd => sd.ServiceType == typeof(IPartManager));
            if (managerDescriptor != null)
            {
                services.Remove(managerDescriptor);
            }
            services.Add(ServiceDescriptor.Singleton<IPartManager>(sp => partManager));
        }
    }
}
