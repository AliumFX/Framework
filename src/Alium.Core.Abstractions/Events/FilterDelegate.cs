// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Events
{
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an event delegate for filtering
    /// </summary>
    /// <typeparam name="TPayload">The payload type</typeparam>
    /// <param name="context">The event context</param>
    /// <returns>The task instance</returns>
    public delegate Task<bool> FilterDelegate<TPayload>(EventContext<TPayload> context);
}
