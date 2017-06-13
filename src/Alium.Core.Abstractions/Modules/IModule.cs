// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    /// <summary>
    /// Defines the required contract for implementing a module.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Gets the system code for the module.
        /// </summary>
        SysCode Code { get; }

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
