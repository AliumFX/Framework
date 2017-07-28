// Copyright (c) Alium Project. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the required contract for implementing a module provider.
    /// </summary>
    public interface IModuleProvider
    {
        /// <summary>
        /// Gets the set of modules.
        /// </summary>
        IReadOnlyCollection<IModule> Modules { get; }
    }
}
