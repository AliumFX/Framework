// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Resolves the current tenant
    /// </summary>
    public class TenantMiddleware : IMiddleware
    {
        private readonly ITenantResolver _tenantResolver;
        private readonly IWorkContext _workContext;

        /// <summary>
        /// Initialises a new instance of <see cref="TenantMiddleware" />
        /// </summary>
        /// <param name="tenantResolver">The tenant resolver</param>
        /// <param name="workContext">The work context</param>
        public TenantMiddleware(ITenantResolver tenantResolver, IWorkContext workContext)
        {
            _tenantResolver = Ensure.IsNotNull(tenantResolver, nameof(tenantResolver));
            _workContext = Ensure.IsNotNull(workContext, nameof(workContext));
        }

        /// <inheritdoc />
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // MA - Resolve the current tenant.
            var tenantId = await _tenantResolver.ResolveCurrentAsync(context);

            if (!tenantId.Equals(TenantId.Empty) && !tenantId.Equals(TenantId.Default))
            {
                // MA - Get the work context
                var extension = new TenantWorkContextExtension(tenantId);
                _workContext.Extensions.SetExtension<ITenantWorkContextExtension>(extension);
            }

            await next(context);
        }
    }
}
