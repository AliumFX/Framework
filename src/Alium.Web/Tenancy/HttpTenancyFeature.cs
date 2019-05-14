// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Hosting;

    using Alium.DependencyInjection;
    using Alium.Features;

    /// <summary>
    /// Describes multi-tenancy for web applications.
    /// </summary>
    public class HttpTenancyFeature : FeatureBase, IServicesBuilder
    {
        /// <summary>
        /// Initialises a new instance of <see cref="HttpTenancyFeature"/>
        /// </summary>
        public HttpTenancyFeature()
            : base(CoreInfo.TenancyFeatureId, "Tenancy", "Provides services for support multi-tenant applications", false)
        { }

        /// <inheritdoc />
        public void BuildServices(IServiceCollection services)
        {
            services.AddSingleton<ITenantResolver<HttpContext>, HttpContextTenantResolver>();
            services.AddSingleton<IStartupFilter, TenantStartupFilter>();
            services.AddScoped<TenantMiddleware>();
        }
    }
}
