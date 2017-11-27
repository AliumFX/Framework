// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    using Alium.Tenancy;

    /// <summary>
    /// Injects middleware to resolve the current tenant
    /// </summary>
    public class TenantStartupFilter : IStartupFilter
    {
        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return (b) =>
            {
                b.UseMiddleware<TenantMiddleware>();

                next(b);
            };
        }
    }
}
