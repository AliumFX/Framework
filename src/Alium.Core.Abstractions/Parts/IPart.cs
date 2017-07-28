// Copyright (c) Alium Project. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Parts
{
    /// <summary>
    /// Defines the required contract for implementing a part.
    /// </summary>
    public interface IPart
    {
        /// <summary>
        /// Gets the part name.
        /// </summary>
        string Name { get; }
    }
}
