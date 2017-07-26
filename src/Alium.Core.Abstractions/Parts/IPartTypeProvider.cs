// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Parts
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Defines the required contract for implementing a part type provider.
    /// </summary>
    public interface IPartTypeProvider
    {
        /// <summary>
        /// Gets the list of available types in the part.
        /// </summary>
        IEnumerable<TypeInfo> Types { get; }
    }
}
