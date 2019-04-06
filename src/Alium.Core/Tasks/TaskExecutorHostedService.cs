// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tasks
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Configures startup and shutdown tasks for the application.
    /// </summary>
    public class TaskExecutorHostedService : IHostedService
    {
        private readonly ITaskExecutor _executor;

        /// <summary>
        /// Initialises a new instance of <see cref="TaskExecutorHostedService"/>.
        /// </summary>
        /// <param name="executor">The task executor.</param>
        /// <param name="applicationLifetime">The application lifetime.</param>
        public TaskExecutorHostedService(ITaskExecutor executor)
        {
            _executor = Ensure.IsNotNull(executor, nameof(executor));
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
            => _executor.ExecuteStartupTasksAsync(cancellationToken);

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
            => _executor.ExecuteShutdownTasksAsync(cancellationToken);
    }
}
