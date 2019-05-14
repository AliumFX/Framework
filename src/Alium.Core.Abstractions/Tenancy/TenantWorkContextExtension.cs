// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    /// <summary>
    /// Provides access to the current tenant id.
    /// </summary>
    public class TenantWorkContextExtension : ITenantWorkContextExtension
    {
        /// <summary>
        /// Initialises a new instance of <see cref="TenantId"/>.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        public TenantWorkContextExtension(TenantId tenantId)
        {
            TenantId = !tenantId.HasValue ? TenantId.Default : tenantId;
        }

        /// <summary>
        /// Gets the tenant id.
        /// </summary>
        public TenantId TenantId { get; }
    }
}
