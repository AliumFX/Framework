// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Provides a facade over services required for events
    /// </summary>
    public class EventServices<TEvent, TPayload> : IEventServices<TEvent, TPayload>
        where TEvent : IEvent<TPayload>
    {
        /// <summary>
        /// Initialises a new instance of <see cref="EventServices{TEvent, TPayload}"/>
        /// </summary>
        /// <param name="subscribers">The set of dynamic subscribers</param>
        /// <param name="subscriptionFactory">The subscription factory</param>
        public EventServices(
            IEnumerable<IEventSubscriber<TEvent, TPayload>> subscribers, 
            IEventSubscriptionFactory<TPayload> subscriptionFactory)
        {
            Subscribers = new ReadOnlyCollection<IEventSubscriber<TEvent, TPayload>>(
                Ensure.IsNotNull(subscribers, nameof(subscribers)).ToList());

            SubscriptionFactory = Ensure.IsNotNull(subscriptionFactory, nameof(subscriptionFactory));
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IEventSubscriber<TEvent, TPayload>> Subscribers { get; }

        /// <inheritdoc />
        public IEventSubscriptionFactory<TPayload> SubscriptionFactory { get; }
    }
}
