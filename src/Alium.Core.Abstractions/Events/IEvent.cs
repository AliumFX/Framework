// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the required contract for implementing an event
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets the event ID
        /// </summary>
        EventId EventId { get; }
    }

    /// <summary>
    /// Defines the required contract for implementing an event
    /// </summary>
    /// <typeparam name="TPayload">The payload type</typeparam>
    public interface IEvent<TPayload>
    {
        /// <summary>
        /// Notifies any attached subscribers of the given payload data
        /// </summary>
        /// <param name="context">The event context</param>
        /// <returns>The task instance</returns>
        Task PublishAsync(EventContext<TPayload> context);

        /// <summary>
        /// Subscribes to the event
        /// </summary>
        /// <param name="onNotificatonAsync">The notification delegate</param>
        /// <param name="onFilterAsync">[Optional] The filter delegate</param>
        /// <returns>A subscription token which can be used to control how long the subscription lasts</returns>
        SubscriptionToken Subscribe(
            NotificationDelegate<TPayload> onNotificatonAsync,
            FilterDelegate<TPayload> onFilterAsync = null);
        
        /// <summary>
        /// Unsubscribes an attached subscriber with the given subscription token
        /// </summary>
        /// <param name="subscriptionToken">The subscription token</param>
        void Unsubscribe(SubscriptionToken subscriptionToken);
    }
}
