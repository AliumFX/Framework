// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;

    /// <summary>
    /// Provides access to the tenant services from a HTTP context.
    /// </summary>
    public class TenantServicesHttpFeature : ITenantServicesHttpFeature
    {
        /// <summary>
        /// Initialises a new instance of <see cref="TenantServicesHttpFeature"/>
        /// </summary>
        /// <param name="services">The tenant service provider</param>
        public TenantServicesHttpFeature(IServiceProvider services = null)
        {
            TenantServices = services;
        }

        /// <inheritdoc />
        public IServiceProvider TenantServices { get; }
    }
}
