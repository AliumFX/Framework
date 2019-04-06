// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Initialises modules.
    /// </summary>
    public class ModuleInitialiserHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IModuleProvider _moduleProvider;

        /// <summary>
        /// Initialises a new instance of <see cref="ModuleInitialiserHostedService"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="moduleProvider">The module provider.</param>
        public ModuleInitialiserHostedService(IServiceProvider serviceProvider, IModuleProvider moduleProvider)
        {
            _serviceProvider = Ensure.IsNotNull(serviceProvider, nameof(serviceProvider));
            _moduleProvider = Ensure.IsNotNull(moduleProvider, nameof(moduleProvider));
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var context = new ModuleInitialisationContext(_serviceProvider);

            foreach (var module in _moduleProvider.Modules)
            {
                module.Initialise(context);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
