// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Provides state for features.
    /// </summary>
    public class FeatureStateProvider : IFeatureStateProvider
    {
        private readonly IConfigurationSection _root;
        private readonly IFeatureProvider _featureProvider;
        private readonly ConcurrentDictionary<FeatureId, FeatureState> _store = new ConcurrentDictionary<FeatureId, FeatureState>();

        /// <summary>
        /// Initialises a new instance of <see cref="FeatureStateProvider"/>.
        /// </summary>
        /// <param name="featureProvider">The feature provider.</param>
        /// <param name="configuration">The configuration.</param>
        public FeatureStateProvider(IFeatureProvider featureProvider, IConfiguration configuration)
        {
            _featureProvider = Ensure.IsNotNull(featureProvider, nameof(featureProvider));
            _root = Ensure.IsNotNull(configuration, nameof(configuration)).GetSection("Features");
        }

        /// <inheritdoc />
        public FeatureState GetFeatureState(FeatureId featureId)
        {
            if (featureId.Equals(FeatureId.Empty))
            {
                throw new ArgumentException("The feature id must be provided and cannot be FeatureId.Empty",
                    nameof(featureId));
            }

            return _store.GetOrAdd(featureId, GetFeatureStateCore);
        }

        private FeatureState GetFeatureStateCore(FeatureId featureId)
        {
            var section = _root?.GetSection(featureId.Value);
            if (section == null)
            {
                return new FeatureState(featureId, null, false);
            }

            return new FeatureState(featureId, section, ResolveEnabled(featureId, section));
        }

        private bool ResolveEnabled(FeatureId featureId, IConfigurationSection section)
        {
            if (!featureId.ParentFeatureId.Equals(FeatureId.Empty))
            {
                var parentState = GetFeatureState(featureId.ParentFeatureId);
                if (parentState == null || !parentState.Enabled)
                {
                    return false;
                }
            }

            if (bool.TryParse(section["Enabled"], out bool enabled))
            {
                return enabled;
            }

            if (bool.TryParse(section.Value, out enabled))
            {
                return enabled;
            }

            var feature = _featureProvider.GetFeature(featureId);
            if (feature != null)
            {
                return feature.EnabledByDefault;
            }

            return false;
        }
    }
}
