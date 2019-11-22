// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Represents a feature-bound service.
    /// </summary>
    public class Feature<TService> : IFeature<TService>
        where TService : class
    {
        /// <summary>
        /// Initialises a new instance of <see cref="Feature{TService}"/>.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="enabled">Is the feature enabled?</param>
        /// <param name="missing">Is the feature missing?</param>
        /// <param name="service">[Optional] The service instance.</param>
        /// <param name="configuration">[Optional] The configuration section.</param>
        public Feature(FeatureId featureId, bool enabled, bool missing, TService? service = default, IConfigurationSection? configuration = null)
        {
            FeatureId = featureId;
            Enabled = enabled;
            Missing = missing;
            Service = service;
            Configuration = configuration;
        }

        /// <inheritdoc />
        public IConfigurationSection? Configuration { get; }

        /// <inheritdoc />
        public virtual bool Enabled { get; }

        /// <inheritdoc />
        public virtual FeatureId FeatureId { get; }

        /// <inheritdoc />
        public virtual bool Missing { get; }

        /// <inheritdoc />
        public virtual TService? Service { get; }
    }

    /// <summary>
    /// Represents a feature-bound service with strongly-typed configuration.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <typeparam name="TConfiguration">The configuration type.</typeparam>
    public class Feature<TService, TConfiguration> : Feature<TService>, IFeature<TService, TConfiguration>
        where TService : class
        where TConfiguration: class, new()
    {
        /// <summary>
        /// Initialises a new instance of <see cref="Feature{TService}"/>.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="enabled">Is the feature enabled?</param>
        /// <param name="missing">Is the feature missing?</param>
        /// <param name="service">[Optional] The service instance.</param>
        /// <param name="configuration">[Optional] The configuration section.</param>
        public Feature(IFeature<TService> service)
            : base(service.FeatureId, service.Enabled, service.Missing, service.Service, service.Configuration)
        {
            Configuration = BindConfiguration();
        }

        /// <inheritdoc />
        public new TConfiguration? Configuration { get; }

        /// <summary>
        /// Binds the configuration.
        /// </summary>
        /// <returns>The configuration isntance.</returns>
        private TConfiguration? BindConfiguration()
        {
            if (base.Configuration is null)
            {
                return default;
            }

            var config = new TConfiguration();

            base.Configuration.Bind(config);

            return config;
        }
    }
}
