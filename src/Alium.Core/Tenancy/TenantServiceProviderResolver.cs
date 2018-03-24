// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides services for resolving service provider instances for specific tenants.
    /// </summary>
    public class TenantServiceProviderResolver : ITenantServiceProviderResolver
    {
        private static readonly ConcurrentDictionary<TenantId, IServiceProvider> _providers
            = new ConcurrentDictionary<TenantId, IServiceProvider>();
        private Lazy<IServiceCollection> _tenantServices;

        /// <summary>
        /// Initialises a new instance of <see cref="TenantServiceProviderResolver"/>
        /// </summary>
        /// <param name="services">The tenant-scoped services</param>
        public TenantServiceProviderResolver(TenantServiceCollectionFactory serviceCollectionFactory)
        {
            Ensure.IsNotNull(serviceCollectionFactory, nameof(serviceCollectionFactory));

            _tenantServices = new Lazy<IServiceCollection>(() => serviceCollectionFactory.CreateServiceCollection(), LazyThreadSafetyMode.PublicationOnly);
        }

        /// <inheritdoc />
        public IServiceProvider GetTenantServiceProvider(TenantId tenantId)
            => _providers.GetOrAdd(tenantId, k => GetTenantServiceProviderCore(tenantId));

        private IServiceProvider GetTenantServiceProviderCore(TenantId tenantId)
            => _tenantServices.Value.BuildServiceProvider();
    }
}
