// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Represents the state of a feature.
    /// </summary>
    public class FeatureState
    {
        /// <summary>
        /// Initialises a new instance of <see cref="FeatureState"/>.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="configurationSection">The configuration section.</param>
        /// <param name="enabled">Enabled state of the feature.</param>
        public FeatureState(FeatureId featureId, IConfigurationSection? configurationSection, bool enabled)
        {
            if (!featureId.HasValue)
            {
                throw new ArgumentException("Feature id is required");
            }

            FeatureId = featureId;
            Enabled = enabled;
            ConfigurationSection = configurationSection;
        }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        public IConfigurationSection? ConfigurationSection { get; }

        /// <summary>
        /// Gets whether the feature is enabled.
        /// </summary>
        public bool Enabled { get; }

        /// <summary>
        /// Gets the feature id.
        /// </summary>
        public FeatureId FeatureId { get; }
    }
}
