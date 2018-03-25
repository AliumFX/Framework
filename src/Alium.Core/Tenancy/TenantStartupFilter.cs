// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Alium.Features;

    /// <summary>
    /// Injects middleware to resolve the current tenant
    /// </summary>
    public class TenantStartupFilter : IStartupFilter
    {
        private ILogger<TenantStartupFilter> _logger;

        /// <summary>
        /// Initialises a new instance of <see cref="TenantStartupFilter"/>
        /// </summary>
        /// <param name="logger">The logger instance</param>
        public TenantStartupFilter(ILogger<TenantStartupFilter> logger)
        {
            _logger = Ensure.IsNotNull(logger, nameof(logger));
        }

        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return (b) =>
            {
                // MA - Let's determine if the tenancy feature is enabled.
                var featureStateProvider = b.ApplicationServices.GetRequiredService<IFeatureStateProvider>();
                if (featureStateProvider.GetFeatureState(CoreInfo.TenancyFeatureId).Enabled)
                {
                    _logger.LogTrace("{FeatureId} is enabled, adding tenant middleware", CoreInfo.TenancyFeatureId);

                    // MA - Flow the tenant middleware.
                    b.UseMiddleware<TenantMiddleware>();
                }
                else
                {
                    _logger.LogTrace("{FeatureId} is disabled, skipping tenant middleware", CoreInfo.TenancyFeatureId);
                }
                
                next(b);
            };
        }
    }
}
