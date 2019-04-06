// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Defines the required contract for implementing a feature-bound service.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    public interface IFeature<TService>
        where TService : class
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        IConfigurationSection? Configuration { get; }

        /// <summary>
        /// Gets whether the feature is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Gets the feature id.
        /// </summary>
        FeatureId FeatureId { get; }

        /// <summary>
        /// Gets whether the feature is missing.
        /// </summary>
        bool Missing { get; }

        /// <summary>
        /// Gets the service.
        /// </summary>
        TService? Service { get; }
    }

    /// <summary>
    /// Defines the required contract for implementing a feature-bound service with strongly-typed configuration.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <typeparam name="TConfiguration">The configuration type.</typeparam>
    public interface IFeature<TService, TConfiguration> : IFeature<TService>
        where TService : class
        where TConfiguration : class, new()
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        new TConfiguration? Configuration { get; }
    }
}
