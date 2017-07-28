// Copyright (c) Alium Project. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Alium.Parts;

    /// <summary>
    /// Provides discovery of module types from application parts.
    /// </summary>
    public class ModulePartFeatureProvider : IPartFeatureProvider<ModulePartFeature>
    {
        private static readonly Type ModuleType = typeof(IModule);

        /// <inheritdoc />
        public void PopulateFeature(IEnumerable<IPart> parts, ModulePartFeature feature)
        {
            Ensure.IsNotNull(parts, nameof(parts));
            Ensure.IsNotNull(feature, nameof(feature));

            foreach (var part in parts.OfType<IPartTypeProvider>())
            {
                foreach (var type in part.Types)
                {
                    if (IsModule(type.AsType()))
                    {
                        feature.ModuleTypes.Add(type);
                    }
                }
            }
        }

        private static bool IsModule(Type type)
        {
            return type.IsPublic
                && !type.IsAbstract
                && ModuleType.IsAssignableFrom(type);
        }
    }
}
