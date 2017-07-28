// Copyright (c) Alium Project. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Parts
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides services for managing parts.
    /// </summary>
    public class PartManager : IPartManager
    {
        /// <inheritdoc />
        public IList<IPartFeatureProvider> PartFeatureProviders { get; } = new List<IPartFeatureProvider>();

        /// <inheritdoc />
        public IList<IPart> Parts { get; } = new List<IPart>();

        /// <inheritdoc />
        public void PopulateFeature<TPartFeature>(TPartFeature feature)
            where TPartFeature : class
        {
            Ensure.IsNotNull(feature, nameof(feature));

            foreach (var provider in PartFeatureProviders.OfType<IPartFeatureProvider<TPartFeature>>())
            {
                provider.PopulateFeature(Parts, feature);
            }
        }
    }
}
