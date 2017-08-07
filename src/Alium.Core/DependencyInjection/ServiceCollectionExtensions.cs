// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.DependencyInjection
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    using Alium.Features;
    using Alium.Modules;

    /// <summary>
    /// Provides extensions for the <see cref="IServiceCollection"/> type.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private static readonly Type _featureFactoryType = typeof(IFeatureFactory<>);
        private static readonly Type _featureType = typeof(IFeature<>);
        private static readonly MethodInfo _getServiceMethod = typeof(IServiceProvider).GetTypeInfo().GetMethod(nameof(IServiceProvider.GetService));

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

            services.AddSingleton(moduleProvider);

            foreach (var module in moduleProvider.Modules)
            {
                if (module is IServicesBuilder builder)
                {
                    builder.BuildServices(services);
                }
            }

            return services;
        }

        /// <summary>
        /// Adds the set of feature services to the collection.
        /// </summary>
        /// <param name="services">The set of services.</param>
        /// <param name="featureProvider">The feature provider.</param>
        /// <param name="featureStateProvider">The feature state provider.</param>
        /// <returns>The set of services.</returns>
        public static IServiceCollection AddFeatureServices(this IServiceCollection services, IFeatureProvider featureProvider, IFeatureStateProvider featureStateProvider)
        {
            Ensure.IsNotNull(services, nameof(services));
            Ensure.IsNotNull(featureProvider, nameof(featureProvider));
            Ensure.IsNotNull(featureStateProvider, nameof(featureStateProvider));

            services.AddSingleton(featureProvider);
            services.AddSingleton(featureStateProvider);

            foreach (var feature in featureProvider.Features)
            {
                if (feature is IServicesBuilder builder)
                {
                    var state = featureStateProvider.GetFeatureState(feature.Id);
                    var featureServices = new ServiceCollection();

                    builder.BuildServices(featureServices);

                    foreach (var descriptor in featureServices)
                    {
                        if (state.Enabled)
                        {
                            services.Add(descriptor);
                        }
                        services.Add(CreateFeatureServiceDescriptor(descriptor, feature));
                    }
                }
            }

            return services;
        }

        private static ServiceDescriptor CreateFeatureServiceDescriptor(ServiceDescriptor descriptor, IFeature feature)
        {
            var featureType = _featureType.MakeGenericType(descriptor.ServiceType);
            var factory = CreateObjectFactory(descriptor, feature);

            return new ServiceDescriptor(featureType, factory, descriptor.Lifetime);
        }

        private static Func<IServiceProvider, object> CreateObjectFactory(ServiceDescriptor descriptor, IFeature feature)
        {
            var factoryType = _featureFactoryType.MakeGenericType(descriptor.ServiceType);
            var method = factoryType.GetMethod("CreateFeature");

            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                return sp => method.Invoke(sp.GetService(factoryType), new object[] { sp, feature.Id });
            }

            var providerParam = Expression.Parameter(typeof(IServiceProvider), "sp");
            var factoryParam = Expression.Constant(factoryType);
            var featureParam = Expression.Constant(feature.Id);

            var resolverCall = Expression.Call(providerParam, _getServiceMethod, factoryParam);
            var cast = Expression.Convert(resolverCall, factoryType);

            var featureCall = Expression.Call(cast, method, providerParam, featureParam);

            var lambda = Expression.Lambda(featureCall, providerParam);

            return (Func<IServiceProvider, object>)lambda.Compile();
        }
    }
}
