// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a base implementation of an event subscriber
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    /// <typeparam name="TPayload">The payload type</typeparam>
    public abstract class EventSubscriberBase<TEvent, TPayload> : IEventSubscriber<TEvent, TPayload>
        where TEvent : IEvent<TPayload>
    {
        private static Task<bool> _cachedTrueResult = Task.FromResult(true);

        /// <inheritdoc />
        public virtual Task<bool> FilterAsync(EventContext<TPayload> context) 
            => _cachedTrueResult;

        /// <inheritdoc />
        public abstract Task NotificationAsync(EventContext<TPayload> context);
    }
}
