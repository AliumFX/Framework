// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Alium.Configuration;
    using Alium.DependencyInjection;
    using Alium.Features;
    using Alium.Infrastructure;
    using Alium.Modules;
    using Alium.Tasks;
    using Alium.Tenancy;
    using System.Collections.Generic;

    /// <summary>
    /// Core module.
    /// </summary>
    public class CoreModule : ModuleBase, IServicesBuilder, IFeaturesBuilder, IAppConfigurationExtender
    {
        /// <summary>
        /// Initialises a new instance of <see cref="CoreModule"/>.
        /// </summary>
        public CoreModule()
            : base(CoreInfo.CoreModuleId, CoreInfo.CoreModuleName, CoreInfo.CoreModuleDescription)
        { }

        /// <inheritdoc />
        public void BuildConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            Ensure.IsNotNull(context, nameof(context));
            Ensure.IsNotNull(builder, nameof(builder));

            builder.AddJsonFile(CoreInfo.FeaturesConfigurationFile, optional: true);
            builder.AddJsonFile(CoreInfo.FlagsConfigurationFile, optional: true);
        }

        /// <inheritdoc />
        public void BuildFeatures(ICollection<IFeature> features)
        {
            features.Add(new TenancyFeature());
        }

        /// <inheritdoc />
        public void BuildServices(IServiceCollection services)
        {
            Ensure.IsNotNull(services, nameof(services));

            services.AddSingleton<IFeatureStateProvider, FeatureStateProvider>();
            services.AddTransient(typeof(IFeatureFactory<>), typeof(FeatureFactory<>));
            services.AddTransient(typeof(IFeature<,>), typeof(Feature<,>));

            services.AddSingleton<IFlags, Flags>();

            services.AddSingleton<ITaskExecutor, TaskExecutor>();
            services.AddSingleton<IStartupFilter, TaskExecutorStartupFilter>();

            services.AddSingleton<IStartupFilter, FeatureInitialiserStartupFilter>();
            services.AddSingleton<IStartupFilter, ModuleInitialiserStartupFilter>();

            services.AddScoped<IWorkContext, WorkContext>();
        }
    }
}
