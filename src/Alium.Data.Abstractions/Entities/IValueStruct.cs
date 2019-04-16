// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Data
{
    /// <summary>
    /// Defines the required contract for implementing a value struct
    /// </summary>
    /// <typeparam name="TValue">The value type</typeparam>
    public interface IValueStruct<TValue>
        where TValue : struct
    {
        /// <summary>
        /// Gets whether the struct has a value
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        /// Gets the struct value
        /// </summary>
        TValue Value { get; }
    }
}
