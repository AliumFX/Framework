// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Hosting;

    using Alium.DependencyInjection;
    using Alium.Features;

    /// <summary>
    /// Describes the multi-tenancy feature.
    /// </summary>
    public class TenancyFeature : FeatureBase, IServicesBuilder
    {
        /// <summary>
        /// Initialises a new instance of <see cref="TenancyFeature"/>
        /// </summary>
        public TenancyFeature()
            : base(CoreInfo.TenancyFeatureId, "Tenancy", "Provides services for support multi-tenant applications", false)
        { }

        /// <inheritdoc />
        public void BuildServices(IServiceCollection services)
        {
            services.AddSingleton<TenantServiceCollectionFactory>();
            services.AddSingleton<ITenantResolver, TenantResolver>();
            services.AddSingleton<ITenantServiceProviderResolver, TenantServiceProviderResolver>();
            services.AddSingleton<IStartupFilter, TenantStartupFilter>();
            services.AddScoped<TenantMiddleware>();
        }
    }
}
