// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the required contract for implementing an event subscriber
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    /// <typeparam name="TPayload">The payload type</typeparam>
    public interface IEventSubscriber<TEvent, TPayload>
    {
        /// <summary>
        /// Filters the incoming event data to determine if the notification should be processed
        /// </summary>
        /// <param name="context">The event context</param>
        /// <returns>The task instance</returns>
        Task<bool> FilterAsync(EventContext<TPayload> context);

        /// <summary>
        /// Handles the notification of new payload data from an event
        /// </summary>
        /// <param name="context">The event context</param>
        /// <returns>The task instance</returns>
        Task NotificationAsync(EventContext<TPayload> context);
    }
}
