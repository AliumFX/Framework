// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Resolves the current tenant.
    /// </summary>
    public interface ITenantResolver
    {
        /// <summary>
        /// Resolves the current tenant id.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>The resolved tenant id.</returns>
        Task<TenantId> ResolveCurrentAsync(HttpContext httpContext);
    }
}
