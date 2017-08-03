// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tasks
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the required contract for implementing a task executor.
    /// </summary>
    public interface ITaskExecutor
    {
        /// <summary>
        /// Executes any startup tasks.
        /// </summary>
        /// <param name="cancellationToken">[Optional] The cancellation token.</param>
        /// <returns>The task instance.</returns>
        Task ExecuteShutdownTasksAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes any shutdown tasks.
        /// </summary>
        /// <param name="cancellationToken">[Optional] The cancellation token.</param>
        /// <returns>The task instance.</returns>
        Task ExecuteStartupTasksAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
