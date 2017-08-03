// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Parts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyModel;

    /// <summary>
    /// An assembly backed part.
    /// </summary>
    public class AssemblyPart : IPart, IPartTypeProvider, ICompilationReferenceProvider
    {
        /// <summary>
        /// Initialises a new instance of <see cref="AssemblyPart"/>.
        /// </summary>
        /// <param name="assembly">The assembly instance.</param>
        public AssemblyPart(Assembly assembly)
        {
            Assembly = Ensure.IsNotNull(assembly, nameof(assembly));
        }

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        public Assembly Assembly { get; }

        /// <inheritdoc />
        public string Name => Assembly.GetName().Name;

        /// <inheritdoc />
        public IEnumerable<TypeInfo> Types => Assembly.DefinedTypes;

        /// <inheritdoc />
        public IEnumerable<string> GetReferencePaths()
        {
            if (Assembly.IsDynamic)
            {
                return Enumerable.Empty<string>();
            }

            var dependencyContext = DependencyContext.Load(Assembly);
            if (dependencyContext != null)
            {
                return dependencyContext.CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());
            }

            return new[] { Assembly.Location };
        }
    }
}
