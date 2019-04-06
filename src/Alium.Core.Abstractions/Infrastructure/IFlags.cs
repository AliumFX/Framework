// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Infrastructure
{
    /// <summary>
    /// Defines the required contract for implementing flags.
    /// </summary>
    public interface IFlags
    {
        /// <summary>
        /// Gets the value for the given flag.
        /// </summary>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="flag">The flag key.</param>
        /// <param name="default">[Optional] the default value.</param>
        /// <returns>The flag value.</returns>
        TValue Value<TValue>(string flag, TValue @default = default(TValue));

        /// <summary>
        /// Gets the value for the given flag.
        /// </summary>
        /// <param name="flag">The flag key.</param>
        /// <param name="default">[Optional] the default value.</param>
        /// <returns>The flag value.</returns>
        string? Value(string flag, string? @default = null);
    }
}
