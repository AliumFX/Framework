// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Initialises feature.
    /// </summary>
    public class FeatureInitialiserHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFeatureProvider _featureProvider;
        private readonly IFeatureStateProvider _featureStateProvider;

        /// <summary>
        /// Initialises a new instance of <see cref="FeatureInitialiserHostedService"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="featureProvider">The feature provider.</param>
        /// <param name="featureStateProvider">The feature state provider.</param>
        public FeatureInitialiserHostedService(
            IServiceProvider serviceProvider,
            IFeatureProvider featureProvider,
            IFeatureStateProvider featureStateProvider)
        {
            _serviceProvider = Ensure.IsNotNull(serviceProvider, nameof(serviceProvider));
            _featureProvider = Ensure.IsNotNull(featureProvider, nameof(featureProvider));
            _featureStateProvider = Ensure.IsNotNull(featureStateProvider, nameof(featureStateProvider));
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var context = new FeatureInitialisationContext(_serviceProvider);

            foreach (var feature in _featureProvider.Features)
            {
                var state = _featureStateProvider.GetFeatureState(feature.Id);
                if (state != null && state.Enabled)
                {
                    feature.Initialise(context);
                }
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
