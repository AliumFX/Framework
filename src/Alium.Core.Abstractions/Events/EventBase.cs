// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a base implementation of an event
    /// </summary>
    /// <typeparam name="TEvent">The self-referencing event type</typeparam>
    /// <typeparam name="TPayload">The payload type</typeparam>
    public abstract class EventBase<TEvent, TPayload> : IEvent<TPayload>
        where TEvent : EventBase<TEvent, TPayload>
    {
        private ConcurrentDictionary<SubscriptionToken, IEventSubscription<TPayload>>? _subscriptions;
        private readonly IEventServices<TEvent, TPayload> _services;

        /// <summary>
        /// Gets the count of registered subscribers
        /// </summary>
        public int SubscriberCount => (_subscriptions?.Count).GetValueOrDefault(0);

        /// <summary>
        /// Initialises a new instance of <see cref="EventBase{TEvent, TPayload}"/>
        /// </summary>
        /// <param name="services">The event services</param>
        protected EventBase(IEventServices<TEvent, TPayload> services)
        {
            _services = Ensure.IsNotNull(services, nameof(services));
        }

        /// <inheritdoc />
        public async Task PublishAsync(EventContext<TPayload> context)
        {
            Ensure.IsNotNull(context, nameof(context));

            // MA - Process any directly added event subscribers
            if (_subscriptions is object)
            {
                List<SubscriptionToken>? unsubscribe = null;

                foreach (var subscription in _subscriptions)
                {
                    var notification = subscription.Value.OnNotificationAsync;
                    var filter = subscription.Value.OnFilterAsync;
                    if (notification is null)
                    {
                        // MA - The subscription is no longer alive, therefore queue up the token to be unsubscribed
                        if (unsubscribe is null)
                        {
                            unsubscribe = new List<SubscriptionToken>();
                        }
                        unsubscribe.Add(subscription.Key);
                        continue;
                    }

                    if (filter is null || await filter(context))
                    {
                        await notification(context);
                    }
                }

                if (unsubscribe is object)
                {
                    foreach (var token in unsubscribe)
                    {
                        Unsubscribe(token);
                    }
                }
            }

            // MA - Process any dynamically resolved subscribers from DI
            if (_services.Subscribers is object && _services.Subscribers.Count > 0)
            {
                foreach (var subscriber in _services.Subscribers)
                {
                    if (await subscriber.FilterAsync(context))
                    {
                        await subscriber.NotificationAsync(context);
                    }
                }
            }
        }

        /// <inheritdoc />
        public SubscriptionToken Subscribe(NotificationDelegate<TPayload> onNotificationAsync, FilterDelegate<TPayload>? onFilterAsync = null)
        {
            Ensure.IsNotNull(onNotificationAsync, nameof(onNotificationAsync));

            var subscriptions = _subscriptions ?? (_subscriptions = new ConcurrentDictionary<SubscriptionToken, IEventSubscription<TPayload>>());
            
            var token = new SubscriptionToken(Unsubscribe);
            var subscription = _services.SubscriptionFactory.CreateEventSubscription(token, onNotificationAsync, onFilterAsync);

            subscriptions.TryAdd(token, subscription);

            return token;
        }

        /// <inheritdoc />
        public void Unsubscribe(SubscriptionToken subscriptionToken)
        {
            Ensure.IsNotNull(subscriptionToken, nameof(subscriptionToken));

            _subscriptions?.TryRemove(subscriptionToken, out IEventSubscription<TPayload> value);
        }
    }
}
