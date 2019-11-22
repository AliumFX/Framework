// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    using Alium.Features;

    /// <summary>
    /// Provides tests for the <see cref="EventsServiceCollectionExtensions"/> type
    /// </summary>
    public class EventsServiceCollectionExtensionTests
    {
        [Fact]
        public void AddEventSubscriber_ValidatesArguments()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("services", () => EventsServiceCollectionExtensions.AddEventSubscriber<TestEvent, string, TestEventSubscriber>(null! /* services */));
        }

        [Fact]
        public void AddEventSubscriber_AddsEventSubscriberAsScopedService()
        {
            RunTests(
                config: services => services.AddEventSubscriber<TestEvent, string, TestEventSubscriber>(),
                predicate: sd => sd.ImplementationType == typeof(TestEventSubscriber)
                                 && sd.ServiceType == typeof(IEventSubscriber<TestEvent, string>)
                                 && sd.Lifetime == ServiceLifetime.Scoped);
        }

        [Fact]
        public void AddEvent_ValidatesArguments()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("services", () => EventsServiceCollectionExtensions.AddEvent<TestEvent, string>(null! /* services */));
        }

        [Fact]
        public void AddEvent_AddsEventAsScopedService()
        {
            RunTests(
                config: services => services.AddEvent<TestEvent, string>(),
                predicate: sd => sd.ImplementationType == typeof(TestEvent) 
                                 && sd.ServiceType == typeof(TestEvent)
                                 && sd.Lifetime == ServiceLifetime.Scoped);
        }

        private void RunTests(
            Action<IServiceCollection> config,
            Func<ServiceDescriptor, bool> predicate,
            Action<IServiceCollection, ServiceDescriptor>? asserts = null)
        {
            // Arrange
            var services = new ServiceCollection();
            config(services);

            // Act

            // Assert
            var descriptor = services.FirstOrDefault(predicate);
            Assert.NotNull(descriptor);

            asserts?.DynamicInvoke(services, descriptor);
        }

        public class TestEvent : EventBase<TestEvent, string>
        {
            public TestEvent(IEventServices<TestEvent, string> services) : base(services)
            {
            }
        }

        public class TestEventSubscriber : EventSubscriberBase<TestEvent, string>
        {
            public override Task NotificationAsync(EventContext<string> context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
