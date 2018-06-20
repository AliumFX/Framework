// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using Alium;
    using Alium.Events;

    /// <summary>
    /// Adds a scoped event subscriber
    /// </summary>
    public static class EventsServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a scoped event
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <typeparam name="TPayload">The payload type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddEvent<TEvent, TPayload>(this IServiceCollection services)
            where TEvent : class, IEvent<TPayload>
        {
            Ensure.IsNotNull(services, nameof(services));

            services.AddScoped<TEvent>();

            return services;
        }

        /// <summary>
        /// Adds a scoped event subscriber
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <typeparam name="TPayload">The payload type</typeparam>
        /// <typeparam name="TSubscriber">The subscriber type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddEventSubscriber<TEvent, TPayload, TSubscriber>(this IServiceCollection services)
            where TEvent : class, IEvent<TPayload>
            where TSubscriber: class, IEventSubscriber<TEvent, TPayload>
        {
            Ensure.IsNotNull(services, nameof(services));

            services.AddScoped<IEventSubscriber<TEvent, TPayload>, TSubscriber>();

            return services;
        }
    }
}
