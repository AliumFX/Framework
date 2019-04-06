// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents an event id
    /// </summary>
    [DebuggerDisplay("Event Id: {Value}")]
    public struct EventId : IComparable<string?>, IComparable<EventId>, IEquatable<string?>, IEquatable<EventId>
    {
        private readonly Lazy<EventId>? _parentEventIdThunk;

        /// <summary>
        /// Represents an empty event id
        /// </summary>
        public static readonly EventId Empty = new EventId();

        /// <summary>
        /// Initialises a new instance of <see cref="EventId"/>
        /// </summary>
        /// <param name="value">The event id value</param>
        public EventId(string value)
        {
            HasValue = true;
            _parentEventIdThunk = null;

            LocalValue = Ensure.IsNotNullOrEmpty(value, nameof(value));
            Value = ResolveValue(Empty, value);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="EventId"/>
        /// </summary>
        /// <param name="parentEventId">The parent event id</param>
        /// <param name="value">The event id value</param>
        public EventId(EventId parentEventId, string value)
        {
            if (parentEventId.Equals(EventId.Empty))
            {
                throw new ArgumentException("The parent event id cannot be EventId.Empty");
            }


            HasValue = true;
            _parentEventIdThunk = new Lazy<EventId>(() => parentEventId);

            LocalValue = Ensure.IsNotNullOrEmpty(value, nameof(value));
            Value = ResolveValue(parentEventId, value);
        }

        /// <summary>
        /// Gets whether the current instance has a value
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the local value of the event id
        /// </summary>
        public string LocalValue { get; }

        /// <summary>
        /// Gets the parent event id
        /// </summary>
        public EventId ParentEventId
        {
            get => _parentEventIdThunk == null ? EventId.Empty : _parentEventIdThunk.Value;
        }

        /// <summary>
        /// Gets the event id value
        /// </summary>
        public string Value { get; }

        /// <inheritdoc />
        public int CompareTo(string? other)
        {
            if (HasValue)
            {
                int stringCompare = StringComparer.OrdinalIgnoreCase.Compare(Value, other);
                return stringCompare < 0
                    ? -1
                    : stringCompare == 0
                        ? 0
                        : 1;
            }

            if (string.IsNullOrEmpty(other))
            {
                return 1;
            }

            return 0;
        }

        /// <inheritdoc />
        public int CompareTo(EventId other)
        {
            if (other.HasValue)
            {
                return CompareTo(other.Value);
            }

            if (HasValue)
            {
                return 1;
            }

            return 0;
        }

        /// <inheritdoc />
        public bool Equals(string? other)
            => CompareTo(other) == 0;

        /// <inheritdoc />
        public bool Equals(EventId other)
            => CompareTo(other) == 0;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is EventId c)
            {
                return Equals(c);
            }
            if (obj is string s)
            {
                return Equals(s);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HasValue ? Value.GetHashCode() : 0;

        /// <inheritdoc />
        public override string ToString()
            => Value;

        /// <summary>
        /// Converts from a <see cref="EventId"/> to a <see cref="string"/>
        /// </summary>
        /// <param name="eventId">The <see cref="EventId"/> value.</param>
        public static explicit operator string(EventId eventId)
            => eventId.Value;

        /// <summary>
        /// Resolves the feature id value.
        /// </summary>
        /// <param name="parentEventId">The parent event id</param>
        /// <param name="value">The local value</param>
        /// <returns>The Resolved value</returns>
        private static string ResolveValue(EventId parentEventId, string value)
        {
            if (parentEventId.Equals(EventId.Empty))
            {
                return value;
            }

            return $"{parentEventId.Value}.{value}";
        }
    }
}
