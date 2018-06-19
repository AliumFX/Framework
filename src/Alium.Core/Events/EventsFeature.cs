// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using Microsoft.Extensions.DependencyInjection;

    using Alium.DependencyInjection;
    using Alium.Features;

    /// <summary>
    /// Describes the events feature.
    /// </summary>
    public class EventsFeature : FeatureBase, IServicesBuilder
    {
        /// <summary>
        /// Initialises a new instance of <see cref="EventsFeature"/>
        /// </summary>
        public EventsFeature()
            : base(CoreInfo.EventsFeatureId, "events", "Provides services for supporting an evented programming model", false)
        { }

        /// <inheritdoc />
        public void BuildServices(IServiceCollection services)
        {
            // MA - These are being registered in CoreModule as features do not support open generics
            //services.AddScoped(typeof(IEventSubscriptionFactory<>), typeof(EventSubscriptionFactory<>));
            //services.AddScoped(typeof(IEventServices<,>), typeof(EventServices<,>));
        }
    }
}
