// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Resolves the current tenant
    /// </summary>
    public class TenantMiddleware : IMiddleware
    {
        private readonly ITenantResolver _tenantResolver;
        private readonly ITenantServiceProviderResolver _tenantServiceProviderResolver;

        /// <summary>
        /// Initialises a new instance of <see cref="TenantMiddleware" />
        /// </summary>
        /// <param name="tenantResolver">The tenant resolver</param>
        /// <param name="workContext">The work context</param>
        /// <param name="tenantServiceProviderResolver">The tenant service provider resolver</param>
        public TenantMiddleware(ITenantResolver tenantResolver, ITenantServiceProviderResolver tenantServiceProviderResolver)
        {
            _tenantResolver = Ensure.IsNotNull(tenantResolver, nameof(tenantResolver));
            _tenantServiceProviderResolver = Ensure.IsNotNull(tenantServiceProviderResolver, nameof(tenantServiceProviderResolver));
        }

        /// <inheritdoc />
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Ensure.IsNotNull(context, nameof(context));
            Ensure.IsNotNull(next, nameof(next));

            IServiceScope? scope = null;
            IServiceProvider? requestServices = null;

            // MA - Resolve the current tenant.
            var tenantId = await _tenantResolver.ResolveCurrentAsync(context);

            if (!tenantId.Equals(TenantId.Empty) && !tenantId.Equals(TenantId.Default))
            {
                // MA - Get the tenant-scoped service provider
                var services = _tenantServiceProviderResolver.GetTenantServiceProvider(tenantId);

                // MA - Create the scope
                scope = services.CreateScope();

                // MA - Flow the scope service provider as the RequestServices instance.
                requestServices = context.RequestServices;
                context.RequestServices = scope.ServiceProvider;

                // MA - Flow the tenant services to the HTTP context.
                var httpFeature = new TenantServicesHttpFeature(services);
                context.Features.Set<ITenantServicesHttpFeature>(httpFeature);

                // MA - Get the work context and flow the tenant ID
                var extension = new TenantWorkContextExtension(tenantId);
                var workContext = scope.ServiceProvider.GetRequiredService<IWorkContext>();
                workContext.Extensions.SetExtension<ITenantWorkContextExtension>(extension);
            }

            await next(context);

            if (scope != null)
            {
                // MA - Remove access to the HTTP feature by replacing it
                context.Features.Set<ITenantServicesHttpFeature>(new TenantServicesHttpFeature());

                // MA - Restore the previous instance of request services
                context.RequestServices = requestServices;

                // MA - Dispose of the scope
                scope.Dispose();
            }
        }
    }
}
