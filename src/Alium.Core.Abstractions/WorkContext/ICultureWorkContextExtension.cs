// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System.Globalization;

    /// <summary>
    /// Defines the required contract for implementing a culture-providing work context extension.
    /// </summary>
    public interface ICultureWorkContextExtension
    {
        /// <summary>
        /// Gets the current culture.
        /// </summary>
        CultureInfo Culture { get; }
    }
}
