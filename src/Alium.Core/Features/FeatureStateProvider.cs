// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Configuration;

    using Alium.Tenancy;

    /// <summary>
    /// Provides state for features.
    /// </summary>
    public class FeatureStateProvider : IFeatureStateProvider
    {
        private readonly IFeatureProvider _featureProvider;
        private readonly IConfigurationSection _featuresConfigurationRoot;
        private readonly IConfigurationSection _tenantsConfigurationRoot;

        private readonly FeatureStateProviderScope _rootScope;
        private readonly ConcurrentDictionary<TenantId, FeatureStateProviderScope> _scopes = new ConcurrentDictionary<TenantId, FeatureStateProviderScope>();

        /// <summary>
        /// Initialises a new instance of <see cref="FeatureStateProvider"/>.
        /// </summary>
        /// <param name="featureProvider">The feature provider.</param>
        /// <param name="configuration">The configuration.</param>
        public FeatureStateProvider(IFeatureProvider featureProvider, IConfiguration configuration)
        {
            _featureProvider = Ensure.IsNotNull(featureProvider, nameof(featureProvider));

            _featuresConfigurationRoot = Ensure.IsNotNull(configuration, nameof(configuration)).GetSection("Features");
            _tenantsConfigurationRoot = configuration.GetSection("Tenants");

            _rootScope = new FeatureStateProviderScope(_featuresConfigurationRoot, featureProvider, null);
        }

        /// <inheritdoc />
        public void BeginTenantScope(TenantId tenantId)
        {
            if (tenantId.Equals(TenantId.Empty) || tenantId.Equals(TenantId.Default))
            {
                return;
            }

            if (!_scopes.TryGetValue(tenantId, out FeatureStateProviderScope scope))
            {
                scope = new FeatureStateProviderScope(_tenantsConfigurationRoot.GetSection($"{tenantId}:Features"), _featureProvider, _rootScope);
                _scopes.TryAdd(tenantId, scope);
            }
        }

        /// <inheritdoc />
        public FeatureState GetFeatureState(FeatureId featureId)
            => _rootScope.GetFeatureState(featureId);

        /// <inheritdoc />
        public FeatureState GetFeatureState(FeatureId featureId, TenantId tenantId)
        {
            if (_scopes.TryGetValue(tenantId, out FeatureStateProviderScope scope))
            {
                return scope.GetFeatureState(featureId);
            }

            return GetFeatureState(featureId);
        }

        private class FeatureStateProviderScope
        {
            private readonly IConfigurationSection _section;
            private readonly IFeatureProvider _featureProvider;
            private readonly ConcurrentDictionary<FeatureId, FeatureState> _store = new ConcurrentDictionary<FeatureId, FeatureState>();

            private FeatureStateProviderScope? _rootScope;

            /// <summary>
            /// Initialises a new instance of <see cref="FeatureStateProviderScope"/>
            /// </summary>
            /// <param name="section">The configuration section.</param>
            /// <param name="featureProvider">The feature provider</param>
            /// <param name="rootScope">[Optional] The root scope.</param>
            public FeatureStateProviderScope(IConfigurationSection section, IFeatureProvider featureProvider, FeatureStateProviderScope? rootScope = null)
            {
                _section = Ensure.IsNotNull(section, nameof(section));
                _featureProvider = Ensure.IsNotNull(featureProvider, nameof(featureProvider));
                _rootScope = rootScope;
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
                var section = _section?.GetSection(featureId.Value);
                if (section == null)
                {
                    if (_rootScope != null)
                    {
                        return _rootScope.GetFeatureState(featureId);
                    }
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

                if (_rootScope != null)
                {
                    return _rootScope.ResolveEnabled(featureId, _rootScope._section);
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
}
