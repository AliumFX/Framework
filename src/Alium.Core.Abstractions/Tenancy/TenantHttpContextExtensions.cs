// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Provides extension methods for the <see cref="HttpContext"/> type
    /// </summary>
    public static class TenantHttpContextExtensions
    {
        /// <summary>
        /// Gets the tenant services for the given HTTP context.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>The service provider</returns>
        public static IServiceProvider GetTenantServices(this HttpContext context)
        {
            Ensure.IsNotNull(context, nameof(context));

            // MA - Get the feature.
            var feature = context.Features.Get<ITenantServicesHttpFeature>();

            // MA - Return the service provider
            return feature?.TenantServices;
        }
    }
}
