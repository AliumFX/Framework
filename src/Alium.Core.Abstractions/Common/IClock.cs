// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;

    /// <summary>
    /// Defines the required contract for implementing a clock.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Gets the current date/time in the UTC timezone.
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}
