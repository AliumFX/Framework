// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the required contract for implementing an event subscription factory
    /// </summary>
    public interface IEventSubscriptionFactory<TPayload>
    {
        /// <summary>
        /// Creates an event subscription
        /// </summary>
        /// <param name="subscriptionToken">The subscription token</param>
        /// <param name="onNotificationAsync">The notification delegate</param>
        /// <param name="onFilterAsync">[Optional] The filter delegate</param>
        /// <returns>The event subscription</returns>
        IEventSubscription<TPayload> CreateEventSubscription(
            SubscriptionToken subscriptionToken,
            NotificationDelegate<TPayload> onNotificationAsync,
            FilterDelegate<TPayload> onFilterAsync = null);
    }
}
