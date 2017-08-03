// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tasks
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Executes lifetime tasks.
    /// </summary>
    public class TaskExecutor : ITaskExecutor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>
        /// Initialises a new instance of <see cref="TaskExecutor"/>.
        /// </summary>
        /// <param name="serviceProvider">The service scope factory.</param>
        public TaskExecutor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = Ensure.IsNotNull(scopeFactory, nameof(scopeFactory));
        }

        /// <inheritdoc />
        public async Task ExecuteShutdownTasksAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tasks = scope.ServiceProvider.GetServices<IShutdownTask>();

                foreach (var task in tasks)
                {
                    await task.ExecuteAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        /// <inheritdoc />
        public async Task ExecuteStartupTasksAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tasks = scope.ServiceProvider.GetServices<IStartupTask>();

                foreach (var task in tasks)
                {
                    await task.ExecuteAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}
