// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a facade over services required for an event
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    /// <typeparam name="TPayload">The payload type</typeparam>
    public interface IEventServices<TEvent, TPayload>
        where TEvent : IEvent<TPayload>
    {
        /// <summary>
        /// Gets the dynamic set of event subscribers
        /// </summary>
        IReadOnlyCollection<IEventSubscriber<TEvent, TPayload>> Subscribers { get; }

        /// <summary>
        /// Gets the subscription factory
        /// </summary>
        IEventSubscriptionFactory<TPayload> SubscriptionFactory { get; }
    }
}
