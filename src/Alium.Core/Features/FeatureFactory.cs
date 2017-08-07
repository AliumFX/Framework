// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides a factory for creating feature-bound service instances.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    public class FeatureFactory<TService> : IFeatureFactory<TService>
    {
        private readonly IFeatureStateProvider _featureStateProvider;

        /// <summary>
        /// Initialises a new instance of <see cref="FeatureFactory{TService}"/>/
        /// </summary>
        /// <param name="featureStateProvider">The feature state provider.</param>
        public FeatureFactory(IFeatureStateProvider featureStateProvider)
        {
            _featureStateProvider = Ensure.IsNotNull(featureStateProvider, nameof(featureStateProvider));
        }

        /// <inheritdoc />
        public IFeature<TService> CreateFeature(IServiceProvider serviceProvider, FeatureId featureId)
        {
            Ensure.IsNotNull(serviceProvider, nameof(serviceProvider));

            var state = _featureStateProvider.GetFeatureState(featureId);
            var service = serviceProvider.GetService<TService>();
            bool missing = Equals(default(TService), service);

            return new Feature<TService>(
                featureId,
                state.Enabled,
                missing,
                service,
                state.ConfigurationSection);
        }
    }
}
