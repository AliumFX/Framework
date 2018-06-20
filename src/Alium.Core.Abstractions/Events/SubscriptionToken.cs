// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System;

    /// <summary>
    /// Represents a token for a subscription
    /// </summary>
    public class SubscriptionToken : DisposableAction<SubscriptionToken>
    {
        /// <summary>
        /// Initialises a new instance of <see cref="SubscriptionToken"/>
        /// </summary>
        /// <param name="unsubscribe">The unsubscribe action</param>
        public SubscriptionToken(Action<SubscriptionToken> unsubscribe)
            : base(unsubscribe) { }

        /// <summary>
        /// Get sthe token used to identify this subscription
        /// </summary>
        public Guid Token { get; } = Guid.NewGuid();
    }
}
