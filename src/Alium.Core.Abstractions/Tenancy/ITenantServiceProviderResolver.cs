// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Defines the required contract for implementing a tenant service provider resolver
    /// </summary>
    public interface ITenantServiceProviderResolver
    {
        /// <summary>
        /// Gets the tenant service provider for the given tenant ID
        /// </summary>
        /// <param name="tenantId">The tenant ID</param>
        /// <returns>The tenant service provider</returns>
        IServiceProvider GetTenantServiceProvider(TenantId tenantId);
    }
}
