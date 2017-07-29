// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;

    using Alium.Features;
    using Alium.Modules;

    /// <summary>
    /// Provides extensions for the <see cref="IServiceCollection"/> type.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the set of module services to the collection.
        /// </summary>
        /// <param name="services">The set of services.</param>
        /// <param name="moduleProvider">The module provider.</param>
        /// <returns>The set of services.</returns>
        public static IServiceCollection AddModuleServices(this IServiceCollection services, IModuleProvider moduleProvider)
        {
            Ensure.IsNotNull(services, nameof(services));
            Ensure.IsNotNull(moduleProvider, nameof(moduleProvider));

            foreach (var module in moduleProvider.Modules)
            {
                if (module is IServicesBuilder builder)
                {
                    // MA - Build any module-specific services.
                    builder.BuildServices(services);
                }

                if (module is IFeatureProvider featureProvider)
                {
                    // MA - Build any feature services.
                    BuildFeatureServices(services, featureProvider);
                }
            }

            return services;
        }

        private static void BuildFeatureServices(IServiceCollection services, IFeatureProvider featureProvider)
        {
            foreach (var feature in featureProvider.GetFeatures())
            {
                if (feature is IServicesBuilder builder)
                {
                    // MA - Build any feature-specific services.
                    builder.BuildServices(services);
                }

                if (feature is IFeatureProvider featureProviderChild)
                {
                    // MA - Build any child feature services.
                    BuildFeatureServices(services, featureProviderChild);
                }
            }
        }
    }
}
