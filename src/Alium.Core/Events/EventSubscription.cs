// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a subscription to an event
    /// </summary>
    /// <typeparam name="TPayload">The payload type</typeparam>
    public class EventSubscription<TPayload> : IEventSubscription<TPayload>
    {
        private readonly WeakReference<NotificationDelegate<TPayload>> _notificationRef;
        private readonly WeakReference<FilterDelegate<TPayload>> _filterRef;

        /// <summary>
        /// Initialises a new instance of <see cref="EventSubscriberBase{TEvent, TPayload}"/>
        /// </summary>
        /// <param name="token">The subscription token</param>
        /// <param name="notification">The notification delegate</param>
        /// <param name="filter">The filter delegate</param>
        public EventSubscription(
            SubscriptionToken token,
            NotificationDelegate<TPayload> notification,
            FilterDelegate<TPayload>? filter = null)
        {
            Token = Ensure.IsNotNull(token, nameof(token));
            _notificationRef = new WeakReference<NotificationDelegate<TPayload>>(
                Ensure.IsNotNull(notification, nameof(notification)));

            filter = filter ?? ((c) => Task.FromResult(true));
            _filterRef = new WeakReference<FilterDelegate<TPayload>>(filter);
        }

        /// <inheritdoc />
        public FilterDelegate<TPayload>? OnFilterAsync
        {
            get
            {
                if (_filterRef.TryGetTarget(out FilterDelegate<TPayload> filter))
                {
                    return filter;
                }

                return null;
            }
        }

        /// <inheritdoc />
        public NotificationDelegate<TPayload>? OnNotificationAsync
        {
            get
            {
                if (_notificationRef.TryGetTarget(out NotificationDelegate<TPayload> notification))
                {
                    return notification;
                }

                return null;
            }
        }

        /// <inheritdoc />
        public SubscriptionToken Token { get; }
    }
}
