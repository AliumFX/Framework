// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a settings for async calls
    /// </summary>
    public readonly struct AsyncSettings
    {
        /// <summary>
        /// Represents empty async settings
        /// </summary>
        public static readonly AsyncSettings Empty = new AsyncSettings();

        /// <summary>
        /// Initialises a new instance of <see cref="AsyncSettings"/>
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="configureAwait">Flag that determines whether to call ConfigureAwait as a tail call to task-readonly based operation</param>
        public AsyncSettings(CancellationToken cancellationToken, bool configureAwait = true)
        {
            CancellationToken = cancellationToken;
            ConfigureAwait = configureAwait;
        }

        /// <summary>
        /// Gets the cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// Gets whether the configure await settings as part of a tail call to a task-based operation
        /// </summary>
        public bool ConfigureAwait { get; }
    }

    /// <summary>
    /// Provides extensions for async task operations
    /// </summary>
    public static class AsyncTaskExtensions
    {
        public static async Task WithSettings(this Task task, AsyncSettings settings)
        {
            if (settings.ConfigureAwait)
            {
                await task.ConfigureAwait(false);
            } else
            {
                await task;
            }
        }

        public static async ValueTask WithSettings(this ValueTask task, AsyncSettings settings)
        {
            if (settings.ConfigureAwait)
            {
                await task.ConfigureAwait(false);
            }
            else
            {
                await task;
            }
        }

        public static async Task<T> WithSettings<T>(this Task<T> task, AsyncSettings settings)
        {
            if (settings.ConfigureAwait)
            {
                return await task.ConfigureAwait(false);
            }
            else
            {
                return await task;
            }
        }

        public static async ValueTask<T> WithSettings<T>(this ValueTask<T> task, AsyncSettings settings)
        {
            if (settings.ConfigureAwait)
            {
                return await task.ConfigureAwait(false);
            }
            else
            {
                return await task;
            }
        }
    }
}
