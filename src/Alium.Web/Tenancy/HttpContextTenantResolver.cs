// Copyright (c) Alium Fx. All rights reserved.
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
    public class HttpContextTenantResolver : ITenantResolver<HttpContext>
    {
        private readonly Lazy<TenantsConfiguration> _configThunk;
        private readonly IFeatureStateProvider _featureStateProvider;
        private readonly FeatureState _tenancyFeatureState;

        /// <summary>
        /// Initialises a new instance of <see cref="HttpContextTenantResolver"/>.
        /// </summary>
        /// <param name="featureStateProvider">The feature state provider.</param>
        /// <param name="configuration">The configuration.</param>
        public HttpContextTenantResolver(IFeatureStateProvider featureStateProvider)
        {
            _featureStateProvider = Ensure.IsNotNull(featureStateProvider, nameof(featureStateProvider));

            _tenancyFeatureState = featureStateProvider.GetFeatureState(CoreInfo.TenancyFeatureId);
            _configThunk = new Lazy<TenantsConfiguration>(() => ResolveConfiguration(_tenancyFeatureState.ConfigurationSection));
        }

        /// <inheritdoc />
        public Task<TenantId> ResolveCurrentAsync(HttpContext context)
        {
            Ensure.IsNotNull(context, nameof(context));

            // MA - Get the tenancy feature state.
            if (!_tenancyFeatureState.Enabled)
            {
                // MA - The default tenant id.
                return Task.FromResult(TenantId.Default);
            }

            // MA - Get the tenants as configuration objects.
            var config = _configThunk.Value;

            if (config.Tenants is null || config.Tenants.Length == 0)
            {
                return Task.FromResult(TenantId.Empty);
            }

            // MA - Resolve against a matching host name.
            string hostname = context.Request.Host.ToString();
            var tenantConfig = config.Tenants.FirstOrDefault(t => t.HostNames is object && t.HostNames.Contains(hostname, StringComparer.OrdinalIgnoreCase));
            if (tenantConfig is object)
            {
                // MA - Create the tenant id.
                var tenantId = new TenantId(tenantConfig.Id);

                // MA - Scope feature states based on the tenant id.
                _featureStateProvider.BeginTenantScope(tenantId);

                return Task.FromResult(tenantId);
            }

            return Task.FromResult(TenantId.Empty);
        }

        private TenantsConfiguration ResolveConfiguration(IConfigurationSection? configuration)
        {
            var config = new TenantsConfiguration();
            var tenants = new List<TenantConfiguration>();

            if (configuration is object)
            {
                foreach (var section in configuration.GetSection("tenants").GetChildren())
                {
                    string id = section.Key;
                    var tenant = new TenantConfiguration(id)
                    {
                        HostNames = section.GetSection("hostnames").AsEnumerable().Skip(1).Select(p => p.Value).ToArray()
                    };

                    tenants.Add(tenant);
                }
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
            public TenantConfiguration[]? Tenants { get; set; }
        }

        private class TenantConfiguration
        {
            /// <summary>
            /// Initialises a new instance of <see cref="TenantConfiguration"/>
            /// </summary>
            /// <param name="id">The tenant ID</param>
            public TenantConfiguration(string id)
            {
                Id = Ensure.IsNotNullOrEmpty(id, nameof(id));
            }

            /// <summary>
            /// Gets or sets the tenant id.
            /// </summary>
            public string Id { get; }

            /// <summary>
            /// Gets or sets the tenant host names.
            /// </summary>
            public string[]? HostNames { get; set; }
        }
    }
}
