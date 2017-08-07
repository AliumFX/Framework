// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    using Alium.DependencyInjection;
    using Alium.Features;
    using Alium.Modules;
    using Alium.Tasks;

    /// <summary>
    /// Core module.
    /// </summary>
    public class CoreModule : ModuleBase, IServicesBuilder
    {
        /// <summary>
        /// Initialises a new instance of <see cref="CoreModule"/>.
        /// </summary>
        public CoreModule()
            : base(CoreInfo.CoreModuleId, CoreInfo.CoreModuleName, CoreInfo.CoreModuleDescription)
        { }

        /// <inheritdoc />
        public void BuildServices(IServiceCollection services)
        {
            Ensure.IsNotNull(services, nameof(services));

            services.AddSingleton<IFeatureStateProvider, FeatureStateProvider>();
            services.AddTransient(typeof(IFeatureFactory<>), typeof(FeatureFactory<>));
            services.AddTransient(typeof(IFeature<,>), typeof(Feature<,>));

            services.AddSingleton<ITaskExecutor, TaskExecutor>();
            services.AddSingleton<IStartupFilter, TaskExecutorStartupFilter>();

            services.AddSingleton<IStartupFilter, ModuleInitialiserStartupFilter>();
        }
    }
}
