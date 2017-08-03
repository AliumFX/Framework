// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Parts
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the required contract for implementing a part manager.
    /// </summary>
    public interface IPartManager
    {
        /// <summary>
        /// Gets the list of part feature providers.
        /// </summary>
        IList<IPartFeatureProvider> PartFeatureProviders { get; }

        /// <summary>
        /// Gets the list of parts.
        /// </summary>
        IList<IPart> Parts { get; }

        /// <summary>
        /// Populates the given part feature.
        /// </summary>
        /// <typeparam name="TPartFeature">The part feature instance.</typeparam>
        /// <param name="feature">The part feature.</param>
        void PopulateFeature<TPartFeature>(TPartFeature feature) 
            where TPartFeature : class;
    }
}
