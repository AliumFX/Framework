// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Infrastructure
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Defines the required contract for implementing an assembly provider.
    /// </summary>
    public interface IAssemblyProvider
    {
        /// <summary>
        /// Gets the set of assemblies.
        /// </summary>
        IReadOnlyCollection<Assembly> Assemblies { get; }
    }
}
