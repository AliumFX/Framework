// Copyright (c) Alium Project. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Provides the set of modules for an application.
    /// </summary>
    public class ModuleProvider : IModuleProvider
    {
        /// <summary>
        /// Initialises a new instance of <see cref="ModuleProvider"/>.
        /// </summary>
        /// <param name="modules">The set of modules.</param>
        public ModuleProvider(IEnumerable<IModule> modules)
        {
            Modules = new ReadOnlyCollection<IModule>(Ensure.IsNotNull(modules, nameof(modules)).ToList());
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IModule> Modules { get; }
    }
}
