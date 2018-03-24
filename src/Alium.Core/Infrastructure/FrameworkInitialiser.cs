// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyModel;

    using Alium.Configuration;
    using Alium.DependencyInjection;
    using Alium.Features;
    using Alium.Modules;
    using Alium.Parts;

    /// <summary>
    /// Initialises framework components.
    /// </summary>
    public class FrameworkInitialiser
    {
        private IFeatureProvider _featureProvider;
        private IFeatureStateProvider _featureStateProvider;
        private IModuleProvider _moduleProvider;
        private IPartManager _partManager;
        private IConfiguration _configuration;

        /// <summary>
        /// Initialises a new instance of <see cref="FrameworkInitialiser"/>.
        /// </summary>
        private FrameworkInitialiser(IConfiguration configuration)
        {
            _configuration = Ensure.IsNotNull(configuration, nameof(configuration));
        }

        /// <summary>
        /// Gets the feature provider.
        /// </summary>
        public IFeatureProvider FeatureProvider => _featureProvider;

        /// <summary>
        /// Gets the feature state provider.
        /// </summary>
        public IFeatureStateProvider FeatureStateProvider => _featureStateProvider;

        /// <summary>
        /// Gets the module provider.
        /// </summary>
        public IModuleProvider ModuleProvider => _moduleProvider;

        /// <summary>
        /// Gets the part manager.
        /// </summary>
        public IPartManager PartManager => _partManager;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration => _configuration;

        /// <summary>
        /// Adds framework services to the service collection.
        /// </summary>
        /// <param name="services">The set of framework services.</param>
        public void AddServices(IServiceCollection services)
        {
            Ensure.IsNotNull(services, nameof(services));

            services.AddSingleton(_partManager);

            services.AddModuleServices(_moduleProvider);
            services.AddFeatureServices(_featureProvider, _featureStateProvider);

            services.AddSingleton<IServiceCollection>(services);
        }

        /// <summary>
        /// Extends the application configuration.
        /// </summary>
        /// <param name="context">The web host builder context.</param>
        /// <param name="builder">The configuration builder.</param>
        public void ExtendConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            Ensure.IsNotNull(context, nameof(context));
            Ensure.IsNotNull(builder, nameof(builder));

            foreach (var module in _moduleProvider.Modules)
            {
                if (module is IAppConfigurationExtender extender)
                {
                    extender.BuildConfiguration(context, builder);
                }
            }

            foreach (var feature in _featureProvider.Features)
            {
                var state = _featureStateProvider.GetFeatureState(feature.Id);
                if (state != null && state.Enabled && feature is IAppConfigurationExtender extender)
                {
                    extender.BuildConfiguration(context, builder);
                }
            }
        }

        /// <summary>
        /// Creates a framework initialiser from a fixed set of modules.
        /// </summary>
        /// <param name="modules">The set of modules.</param>
        /// <param name="configuration">The framework configuration.</param>
        /// <returns>The framework initialiser.</returns>
        public static FrameworkInitialiser FromModules(IModule[] modules, IConfiguration configuration)
        {
            Ensure.IsNotNull(modules, nameof(modules));

            var init = new FrameworkInitialiser(configuration);
            init._moduleProvider = new ModuleProvider(modules);
            init._partManager = new PartManager();
            init._partManager.PartFeatureProviders.Add(new ModulePartFeatureProvider());
            
            foreach (var assembly in init._moduleProvider.Modules.Select(m => m.GetType().Assembly))
            {
                init._partManager.Parts.Add(new AssemblyPart(assembly));
            }

            init._featureProvider = CreateFeatureProvider(init._moduleProvider);
            init._featureStateProvider = new FeatureStateProvider(init._featureProvider, init._configuration);

            return init;
        }

        /// <summary>
        /// Creates a framework initialiser from discovered modules provided by a dependency context.
        /// </summary>
        /// <param name="context">The dependency context.</param>
        /// <param name="configuration">The framework configuration.</param>
        /// <returns>The framework initialiser.</returns>
        public static FrameworkInitialiser FromDependencyContext(DependencyContext context, IConfiguration configuration)
        {
            var assemblyProvider = new DependencyContextAssemblyProvider(context);

            var init = new FrameworkInitialiser(configuration);
            init._partManager = new PartManager();
            init._partManager.PartFeatureProviders.Add(new ModulePartFeatureProvider());

            foreach (var assembly in assemblyProvider.Assemblies)
            {
                init._partManager.Parts.Add(new AssemblyPart(assembly));
            }

            var feature = new ModulePartFeature();
            init._partManager.PopulateFeature(feature);

            init._moduleProvider = new ModuleProvider(
                feature.ModuleTypes.Select(t => (IModule)Activator.CreateInstance(t)));

            init._featureProvider = CreateFeatureProvider(init._moduleProvider);
            init._featureStateProvider = new FeatureStateProvider(init._featureProvider, init._configuration);

            return init;
        }

        private static IFeatureProvider CreateFeatureProvider(IModuleProvider moduleProvider)
        {
            var features = new List<IFeature>();

            foreach (var module in moduleProvider.Modules)
            {
                if (module is IFeaturesBuilder builder)
                {
                    builder.BuildFeatures(features);
                }
            }

            return new FeatureProvider(features);
        }
    }
}
