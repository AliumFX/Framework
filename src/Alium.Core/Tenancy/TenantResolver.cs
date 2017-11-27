﻿// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    using Alium.Features;

    /// <summary>
    /// Provides services to resolve the current tenant.
    /// </summary>
    public class TenantResolver : ITenantResolver
    {
        private readonly Lazy<TenantsConfiguration> _configThunk = null;
        private readonly IFeatureStateProvider _featureStateProvider = null;
        private readonly FeatureState _tenancyFeatureState;

        /// <summary>
        /// Initialises a new instance of <see cref="TenantResolver"/>.
        /// </summary>
        /// <param name="featureStateProvider">The feature state provider.</param>
        /// <param name="configuration">The configuration.</param>
        public TenantResolver(IFeatureStateProvider featureStateProvider)
        {
            _featureStateProvider = Ensure.IsNotNull(featureStateProvider, nameof(featureStateProvider));

            _tenancyFeatureState = featureStateProvider.GetFeatureState(CoreInfo.TenancyFeatureId);
            _configThunk = new Lazy<TenantsConfiguration>(() => ResolveConfiguration(_tenancyFeatureState.ConfigurationSection));
        }

        /// <inheritdoc />
        public Task<TenantId> ResolveCurrentAsync(HttpContext httpContext)
        {
            Ensure.IsNotNull(httpContext, nameof(httpContext));

            // MA - Get the tenancy feature state.
            if (!_tenancyFeatureState.Enabled)
            {
                // MA - The default tenant id.
                return Task.FromResult(TenantId.Default);
            }

            // MA - Get the tenants as configuration objects.
            var config = _configThunk.Value;

            if (config.Tenants == null || config.Tenants.Length == 0)
            {
                return Task.FromResult(TenantId.Empty);
            }

            // MA - Resolve against a matching host name.
            string hostname = httpContext.Request.Host.ToString();
            var tenantConfig = config.Tenants.FirstOrDefault(t => t.HostNames != null && t.HostNames.Contains(hostname, StringComparer.OrdinalIgnoreCase));
            if (tenantConfig != null)
            {
                // MA - Create the tenant id.
                var tenantId = new TenantId(tenantConfig.Id);

                // MA - Scope feature states based on the tenant id.
                _featureStateProvider.BeginTenantScope(tenantId);

                return Task.FromResult(tenantId);
            }

            return Task.FromResult(TenantId.Empty);
        }

        private TenantsConfiguration ResolveConfiguration(IConfigurationSection configuration)
        {
            var config = new TenantsConfiguration();
            var tenants = new List<TenantConfiguration>();

            foreach (var section in configuration.GetSection("tenants").GetChildren())
            {
                string id = section.Key;
                var tenant = new TenantConfiguration()
                {
                    Id = id,
                    HostNames = section.GetSection("hostnames").AsEnumerable().Skip(1).Select(p => p.Value).ToArray()
                };

                tenants.Add(tenant);
            }

            return new TenantsConfiguration
            {
                Tenants = tenants.ToArray()
            };
        }

        private class TenantsConfiguration
        {
            /// <summary>
            /// Gets or sets the set of tenants.
            /// </summary>
            public TenantConfiguration[] Tenants { get; set; }
        }

        private class TenantConfiguration
        {
            /// <summary>
            /// Gets or sets the tenant id.
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the tenant host names.
            /// </summary>
            public string[] HostNames { get; set; }
        }
    }
}
