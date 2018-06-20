// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    /// <summary>
    /// Provides services for creating instances of event subscriptions
    /// </summary>
    /// <typeparam name="TPayload">The payload type</typeparam>
    public class EventSubscriptionFactory<TPayload> : IEventSubscriptionFactory<TPayload>
    {
        /// <inheritdoc />
        public IEventSubscription<TPayload> CreateEventSubscription(
            SubscriptionToken subscriptionToken, 
            NotificationDelegate<TPayload> onNotificationAsync, 
            FilterDelegate<TPayload> onFilterAsync = null)
            => new EventSubscription<TPayload>(subscriptionToken, onNotificationAsync, onFilterAsync);
    }
}
