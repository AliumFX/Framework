// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Initialises modules.
    /// </summary>
    public class ModuleInitialiserStartupFilter : IStartupFilter
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IModuleProvider _moduleProvider;

        /// <summary>
        /// Initialises a new instance of <see cref="ModuleInitialiserStartupFilter"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="moduleProvider">The module provider.</param>
        public ModuleInitialiserStartupFilter(IServiceProvider serviceProvider, IModuleProvider moduleProvider)
        {
            _serviceProvider = Ensure.IsNotNull(serviceProvider, nameof(serviceProvider));
            _moduleProvider = Ensure.IsNotNull(moduleProvider, nameof(moduleProvider));
        }

        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            Ensure.IsNotNull(next, nameof(next));
            
            var context = new ModuleInitialisationContext(_serviceProvider);

            foreach (var module in _moduleProvider.Modules)
            {
                module.Initialise(context);
            }

            return next;
        }
    }
}
