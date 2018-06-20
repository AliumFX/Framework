// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    /// <summary>
    /// Defines the required contract for implementing an event subscription
    /// </summary>
    /// <typeparam name="TPayload">The payload type</typeparam>
    public interface IEventSubscription<TPayload>
    {
        /// <summary>
        /// Gets the filter delegate
        /// </summary>
        FilterDelegate<TPayload> OnFilterAsync { get; }

        /// <summary>
        /// Gets the notification delegate
        /// </summary>
        NotificationDelegate<TPayload> OnNotificationAsync { get; }

        /// <summary>
        /// Gets the subscription token
        /// </summary>
        SubscriptionToken Token { get; }
    }
}
