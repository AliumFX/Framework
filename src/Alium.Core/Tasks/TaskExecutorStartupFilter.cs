// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tasks
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Configures startup and shutdown tasks for the application.
    /// </summary>
    public class TaskExecutorStartupFilter : IStartupFilter
    {
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly ITaskExecutor _executor;

        /// <summary>
        /// Initialises a new instance of <see cref="TaskExecutorStartupFilter"/>.
        /// </summary>
        /// <param name="executor">The task executor.</param>
        /// <param name="applicationLifetime">The application lifetime.</param>
        public TaskExecutorStartupFilter(ITaskExecutor executor, IApplicationLifetime applicationLifetime)
        {
            _executor = Ensure.IsNotNull(executor, nameof(executor));
            _applicationLifetime = Ensure.IsNotNull(applicationLifetime, nameof(applicationLifetime));
        }

        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            Ensure.IsNotNull(next, nameof(next));

            // MA - Execute any startup tasks.
            _executor.ExecuteStartupTasksAsync().GetAwaiter().GetResult();

            // MA - Attach to the application lifetime to trigger any shutdown tasks.
            _applicationLifetime.ApplicationStopped.Register(() =>
                _executor.ExecuteShutdownTasksAsync().GetAwaiter().GetResult());

            return next;
        }
    }
}
