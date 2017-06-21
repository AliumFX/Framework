// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;

    /// <summary>
    /// Provides a base implementation of a module.
    /// </summary>
    public abstract class ModuleBase : IModule
    {
        /// <summary>
        /// Initialises a new instance of <see cref="ModuleBase"/>.
        /// </summary>
        /// <param name="id">The module id.</param>
        /// <param name="name">[Optional] The module name.</param>
        /// <param name="description">[Optional] The module description.</param>
        protected ModuleBase(ModuleId id, string name = null, string description = null)
        {
            if (id.Equals(ModuleId.Empty))
            {
                throw new ArgumentException("The module id value must be provided", nameof(id));
            }

            Id = id;
            Name = name;
            Description = description;
        }

        /// <inheritdoc />
        public ModuleId Id { get; }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public virtual void Initialise() { }
    }
}
