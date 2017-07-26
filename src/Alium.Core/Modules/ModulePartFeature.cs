// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Represents a feature which providers a set of module types.
    /// </summary>
    public class ModulePartFeature
    {
        /// <summary>
        /// Gets the set of module types.
        /// </summary>
        public IList<TypeInfo> ModuleTypes { get; } = new List<TypeInfo>();
    }
}
