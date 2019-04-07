// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System;
    using System.Threading.Tasks;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="EventSubscription{TPayload}"/> type
    /// </summary>
    public class EventSubscriptionTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange
            var token = new SubscriptionToken(st => { });
            Task notification(EventContext<object> context) => Task.CompletedTask;

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>("token", () => new EventSubscription<object>(
                null /* token */,
                notification));
            Assert.Throws<ArgumentNullException>("notification", () => new EventSubscription<object>(
                token,
                null /* notification */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void Constructor_AssignsValues()
        {
            // Arrange
            var token = new SubscriptionToken(st => { });
            NotificationDelegate<object> notification = c => Task.CompletedTask;
            FilterDelegate<object> filter = c => Task.FromResult(true);

            // Act
            var subscription = new EventSubscription<object>(
                token,
                notification,
                filter);

            // Assert
            Assert.Same(token, subscription.Token);
            Assert.Same(notification, subscription.OnNotificationAsync);
            Assert.Same(filter, subscription.OnFilterAsync);
        }

        [Fact]
        public void Constructor_ProvidesDefaultFilter()
        {
            // Arrange
            var token = new SubscriptionToken(st => { });
            NotificationDelegate<object> notification = c => Task.CompletedTask;

            // Act
            var subscription = new EventSubscription<object>(
                token,
                notification);

            // Assert
            Assert.NotNull(subscription.OnFilterAsync);
        }

        [Fact]
        public void OnNotification_ReturnsValueForLiveSubscriber()
        {
            // Arrange
            var token = new SubscriptionToken(st => { });
            NotificationDelegate<object> notification = c => Task.CompletedTask;

            // Act
            var subscription = new EventSubscription<object>(
                token,
                notification);

            // Assert
            Assert.NotNull(subscription.OnFilterAsync);
        }
    }
}
