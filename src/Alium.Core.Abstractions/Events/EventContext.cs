// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System.Threading;

    /// <summary>
    /// Represents a context for an event
    /// </summary>
    public class EventContext
    {
        /// <summary>
        /// Initialises a new instance of <see cref="EventContext"/>
        /// </summary>
        /// <param name="eventId">The event ID</param>
        /// <param name="cancellationToken">[Optional] The cancellation token</param>
        public EventContext(EventId eventId, CancellationToken cancellationToken = default(CancellationToken))
        {
            EventId = eventId;
            EventAborted = cancellationToken;
        }

        /// <summary>
        /// Gets the cancellation token for controlling when an event is aborted
        /// </summary>
        public CancellationToken EventAborted { get; }

        /// <summary>
        /// Gets the event ID
        /// </summary>
        public EventId EventId { get; }
    }

    /// <summary>
    /// Represents the context for an event
    /// </summary>
    /// <typeparam name="TPayload">The payload type</typeparam>
    public class EventContext<TPayload> : EventContext
    {
        /// <summary>
        /// Initialises a new instance of <see cref="EventContext{TPayload}"/>
        /// </summary>
        /// <param name="eventId">The event ID</param>
        /// <param name="payload">The payload data</param>
        /// <param name="cancellationToken">[Optional] The cancellation token</param>
        public EventContext(EventId eventId, TPayload payload, CancellationToken cancellationToken = default(CancellationToken))
            : base(eventId, cancellationToken)
        {
            Payload = payload;
        }

        /// <summary>
        /// Gets the payload
        /// </summary>
        public TPayload Payload { get; }
    }
}
