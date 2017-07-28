// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the required contract for implementing a module.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Gets the set of dependencies for this module.
        /// </summary>
        IReadOnlyCollection<ModuleId> Dependencies { get; }

        /// <summary>
        /// Gets the module id.
        /// </summary>
        ModuleId Id { get; }

        /// <summary>
        /// Gets the description of the module.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Initialises the module.
        /// </summary>
        void Initialise();
    }
}
