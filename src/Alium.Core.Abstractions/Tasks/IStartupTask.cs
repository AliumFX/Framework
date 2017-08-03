// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tasks
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the required contract for implementing a startup task.
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        /// Fired when the application is starting up.
        /// </summary>
        /// <param name="cancellationToken">[Optional] The cancellation token.</param>
        /// <returns>The task instance.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
