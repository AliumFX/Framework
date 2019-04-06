// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="EventId"/> type
    /// </summary>
    public class EventIdTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentException>(() => new EventId(null));
            Assert.Throws<ArgumentException>(() => new EventId(string.Empty));
            Assert.Throws<ArgumentException>(() => new EventId(EventId.Empty, null));
            Assert.Throws<ArgumentException>(() => new EventId(new EventId("Test"), null));
            Assert.Throws<ArgumentException>(() => new EventId(new EventId("Test"), string.Empty));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void InitialisedInstance_HasValue_WhenProvidingValue()
        {
            // Arrange

            // Act
            var eventId = new EventId("Test");

            // Assert
            Assert.True(eventId.HasValue);
            Assert.Equal("Test", eventId.LocalValue);
            Assert.Equal("Test", eventId.Value);
            Assert.Equal(EventId.Empty, eventId.ParentEventId);
        }

        [Fact]
        public void InitialisedInstance_HasValue_WhenProvidingParentEventIdAndValue()
        {
            // Arrange
            var parentEventId = new EventId("ParentEvent");

            // Act
            var eventId = new EventId(parentEventId, "Test");

            // Assert
            Assert.True(eventId.HasValue);
            Assert.Equal("Test", eventId.LocalValue);
            Assert.Equal("ParentEvent.Test", eventId.Value);
            Assert.Equal(parentEventId, eventId.ParentEventId);
        }

        [Fact]
        public void CanCompare_AgainstEventId()
        {
            // Arrange
            var eventId = new EventId("event");

            // Act
            int compare0 = eventId.CompareTo(EventId.Empty);
            int compare1 = eventId.CompareTo(new EventId("event"));
            int compare2 = eventId.CompareTo(new EventId("zzzz"));
            int compare3 = eventId.CompareTo(new EventId("aaaa"));

            // Assert
            Assert.True(1 == compare0);
            Assert.True(0 == compare1);
            Assert.True(-1 == compare2);
            Assert.True(1 == compare3);
        }

        [Fact]
        public void CanCompare_AgainstString()
        {
            // Arrange
            var eventId = new EventId("event");

            // Act
            int compare0 = eventId.CompareTo((string?)null);
            int compare1 = eventId.CompareTo("event");
            int compare2 = eventId.CompareTo("zzzz");
            int compare3 = eventId.CompareTo("aaaa");

            // Assert
            Assert.True(1 == compare0);
            Assert.True(0 == compare1);
            Assert.True(-1 == compare2);
            Assert.True(1 == compare3);
        }

        [Fact]
        public void WhenComparing_UsesCaseInsenstiveCompare()
        {
            // Arrange
            string value = "event";
            var eventId = new EventId(value);

            // Act
            int compare = eventId.CompareTo("EVENT");

            // Assert
            Assert.Equal(0, compare);
        }

        [Fact]
        public void CanEquate_AgainstEventId()
        {
            // Arrange
            var eventId = new EventId("event");

            // Act
            bool equate0 = eventId.Equals(EventId.Empty);
            bool equate1 = eventId.Equals(new EventId("event"));
            bool equate2 = eventId.Equals(new EventId("aaaa"));

            // Assert
            Assert.False(equate0);
            Assert.True(equate1);
            Assert.False(equate2);
        }

        [Fact]
        public void CanEquate_AgainstString()
        {
            // Arrange
            var eventId = new EventId("event");

            // Act
            bool equate0 = eventId.Equals((string?)null);
            bool equate1 = eventId.Equals("event");
            bool equate2 = eventId.Equals("aaaa");

            // Assert
            Assert.False(equate0);
            Assert.True(equate1);
            Assert.False(equate2);
        }

        [Fact]
        public void WhenEquating_UsesCaseInsensitiveCompare()
        {
            // Arrange
            var eventId = new EventId("event");

            // Act
            bool equals = eventId.Equals(new EventId("event"));

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void CanExplicitlyCase_FromEventId_ToString()
        {
            // Arrange
            var eventId = new EventId("event");

            // Act
            string value = (string)eventId;

            // Assert
            Assert.Equal("event", value);
        }
    }
}
