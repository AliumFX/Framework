// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="EventBase{TEvent, TPayload}"/> type
    /// </summary>
    public class EventBaseTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>(() => new TestEvent(null /* services */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public async Task PublishAsync_ValidatesArguments()
        {
            // Arrange
            var services = CreateServices();
            var @event = new TestEvent(services);

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await @event.PublishAsync(null /* context */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public async Task PublishAsync_NotifiesAttachedSubscribers_WithNoFilter()
        {
            // Arrange
            EventContext<object>? captured = null;

            var payload = new Object();
            var eventId = new EventId("Test");
            var context = new EventContext<object>(eventId, payload);

            var services = CreateServices();
            var @event = new TestEvent(services);
            var token = @event.Subscribe(c =>
            {
                captured = c;
                return Task.CompletedTask;
            });

            // Act
            await @event.PublishAsync(context);

            // Assert
            Assert.NotNull(captured);
            Assert.Same(context, captured);
            Assert.Same(payload, captured?.Payload);
            Assert.Equal(eventId, captured?.EventId);
        }

        [Fact]
        public async Task PublishAsync_NotifiesAttachedSubscribers_WithTrueFilter()
        {
            // Arrange
            EventContext<object>? captured = null;

            var payload1 = new Object();
            var eventId1 = new EventId("Test1");
            var context1 = new EventContext<object>(eventId1, payload1);

            var payload2 = new Object();
            var eventId2 = new EventId("Test2");
            var context2 = new EventContext<object>(eventId2, payload2);

            var services = CreateServices();
            var @event = new TestEvent(services);
            var token1 = @event.Subscribe(c =>
            {
                captured = c;
                return Task.CompletedTask;
            } /* notification */,
            c => Task.FromResult(c.EventId.Equals(eventId1)));
            var token2 = @event.Subscribe(c =>
            {
                captured = c;
                return Task.CompletedTask;
            } /* notification */,
            c => Task.FromResult(c.EventId.Equals(eventId2)));

            // Act
            await @event.PublishAsync(context1);

            // Assert
            Assert.NotNull(captured);
            Assert.Same(context1, captured);
            Assert.Same(payload1, captured?.Payload);
            Assert.Equal(eventId1, captured?.EventId);
        }

        [Fact]
        public async Task PublishAsync_NotifiesDynamicSubscribers_WithNoFilter()
        {
            // Arrange
            EventContext<object>? captured = null;

            var payload = new Object();
            var eventId = new EventId("Test");
            var context = new EventContext<object>(eventId, payload);

            var services = CreateServices(
                subscribers: new[] {
                    CreateSubscriber(
                        onNotification: c =>
                        {
                            captured = c;
                        }
                    )
                });
            var @event = new TestEvent(services);

            // Act
            await @event.PublishAsync(context);

            // Assert
            Assert.NotNull(captured);
            Assert.Same(context, captured);
            Assert.Same(payload, captured?.Payload);
            Assert.Equal(eventId, captured?.EventId);
        }

        [Fact]
        public async Task PublishAsync_NotifiesDynamicSubscribers_WithTrueFilter()
        {
            // Arrange
            EventContext<object>? captured = null;

            var payload1 = new Object();
            var eventId1 = new EventId("Test1");
            var context1 = new EventContext<object>(eventId1, payload1);

            var payload2 = new Object();
            var eventId2 = new EventId("Test2");
            var context2 = new EventContext<object>(eventId2, payload2);

            var services = CreateServices(
                subscribers: new[] {
                    CreateSubscriber(
                        onNotification: c =>
                        {
                            captured = c;
                        },
                        onFilter: c => c.EventId.Equals(eventId1)
                    ),
                    CreateSubscriber(
                        onNotification: c =>
                        {
                            captured = c;
                        },
                        onFilter: c => c.EventId.Equals(eventId2)
                    )
                });
            var @event = new TestEvent(services);

            // Act
            await @event.PublishAsync(context1);

            // Assert
            Assert.NotNull(captured);
            Assert.Same(context1, captured);
            Assert.Same(payload1, captured?.Payload);
            Assert.Equal(eventId1, captured?.EventId);
        }

        [Fact]
        public async Task PublishAsync_RemovesDeadSubscriptions()
        {
            // Arrange
            EventContext<object>? captured = null;

            var payload = new Object();
            var eventId = new EventId("Test");
            var context = new EventContext<object>(eventId, payload);

            var services = CreateServices(isAlive: false);
            var @event = new TestEvent(services);
            var token = @event.Subscribe(c =>
            {
                captured = c;
                return Task.CompletedTask;
            });

            // Act
            Assert.Equal(1, @event.SubscriberCount);
            await @event.PublishAsync(context);

            // Assert
            Assert.Null(captured);
            Assert.Equal(0, @event.SubscriberCount);
        }

        [Fact]
        public void Subscribe_ValidatesArguments()
        {
            // Arrange
            var services = CreateServices();
            var @event = new TestEvent(services);

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>(() => @event.Subscribe(null /* onNotificationAsync */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void Subscribe_CreatesSubscription()
        {
            // Arrange
            var services = CreateServices(onEventSubscriptionNotification: c => { });
            var @event = new TestEvent(services);

            // Act & Assert
            Assert.Equal(0, @event.SubscriberCount);
            var token = @event.Subscribe(c => Task.FromResult(true));
            
            Assert.NotNull(token);
            Assert.Equal(1, @event.SubscriberCount);
        }

        [Fact]
        public void Unsubscribe_ValidatesArguments()
        {
            // Arrange
            var services = CreateServices();
            var @event = new TestEvent(services);

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>(() => @event.Unsubscribe(null /* subscriptioknToken */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void Unsubscribe_RemovesSubscription()
        {
            // Arrange
            var services = CreateServices(onEventSubscriptionNotification: c => { });
            var @event = new TestEvent(services);

            // Act & Assert
            Assert.Equal(0, @event.SubscriberCount);
            var token = @event.Subscribe(c => Task.FromResult(true));
            Assert.Equal(1, @event.SubscriberCount);

            @event.Unsubscribe(new SubscriptionToken(st => { }));
            Assert.Equal(1, @event.SubscriberCount);

            @event.Unsubscribe(token);
            Assert.Equal(0, @event.SubscriberCount);
        }

        [Fact]
        public void Unsubscribe_ExecutedByDisposingSubscriptionToken()
        {
            // Arrange
            var services = CreateServices(onEventSubscriptionNotification: c => { });
            var @event = new TestEvent(services);

            // Act & Assert
            Assert.Equal(0, @event.SubscriberCount);
            var token = @event.Subscribe(c => Task.FromResult(true));
            Assert.Equal(1, @event.SubscriberCount);

            token.Dispose();
            Assert.Equal(0, @event.SubscriberCount);
        }


        private IEventServices<TestEvent, object> CreateServices(
            IEnumerable<IEventSubscriber<TestEvent, object>>? subscribers = null,
            Func<EventContext<object>, bool>? onEventSubscriptionFilter = null,
            Action<EventContext<object>>? onEventSubscriptionNotification = null,
            bool isAlive = true)
        {
            var mock = new Mock<IEventServices<TestEvent, object>>();

            mock.Setup(m => m.Subscribers)
                .Returns(() => 
                    new ReadOnlyCollection<IEventSubscriber<TestEvent, object>>(
                        (subscribers ?? Enumerable.Empty<IEventSubscriber<TestEvent, object>>()).ToList()
                    )
                );

            mock.Setup(m => m.SubscriptionFactory)
                .Returns(() => CreateSubscriptionFactory(onEventSubscriptionFilter, onEventSubscriptionNotification, isAlive));

            return mock.Object;
        }

        private IEventSubscriber<TestEvent, object> CreateSubscriber(
            Func<EventContext<object>, bool>? onFilter = null,
            Action<EventContext<object>>? onNotification = null)
        {
            var mock = new Mock<IEventSubscriber<TestEvent, object>>();

            mock.Setup(m => m.FilterAsync(It.IsAny<EventContext<object>>()))
                .Returns<EventContext<object>>(c =>
                {
                    if (onFilter != null)
                    {
                        return Task.FromResult(onFilter(c));
                    }
                    return Task.FromResult(true);
                });

            mock.Setup(m => m.NotificationAsync(It.IsAny<EventContext<object>>()))
                .Returns<EventContext<object>>(c =>
                {
                    if (onNotification != null)
                    {
                        onNotification(c);
                    }

                    return Task.CompletedTask;
                });

            return mock.Object;
        }

        private IEventSubscriptionFactory<object> CreateSubscriptionFactory(
            Func<EventContext<object>, bool>? onEventSubscriptionFilter = null,
            Action<EventContext<object>>? onEventSubscriptionNotification = null,
            bool isAlive = true)
        {
            var mock = new Mock<IEventSubscriptionFactory<object>>();

            mock.Setup(m => m.CreateEventSubscription(
                    It.IsAny<SubscriptionToken>(),
                    It.IsAny<NotificationDelegate<object>>(),
                    It.IsAny<FilterDelegate<object>>()))
                .Returns<SubscriptionToken, NotificationDelegate<object>, FilterDelegate<object>>(
                    (t, n, f) =>
                    {
                        return new TestEventSubscription(t)
                        {
                            OnFilterAsync = f,
                            OnNotificationAsync = isAlive ? n : null
                        };
                    });

            return mock.Object;
        }

        public class TestEvent : EventBase<TestEvent, object>
        {
            public TestEvent(IEventServices<TestEvent, object> services)
                : base(services) { }
        }

        public class TestEventSubscription : IEventSubscription<object>
        {
            public TestEventSubscription(SubscriptionToken token)
            {
                Token = token;
            }

            public FilterDelegate<object>? OnFilterAsync { get; set; }
            public NotificationDelegate<object>? OnNotificationAsync { get; set; }
            public SubscriptionToken Token { get; set; }
        }
    }
}
