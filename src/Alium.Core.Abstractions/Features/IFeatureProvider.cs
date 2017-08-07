// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the required contract for implementing a feature provider.
    /// </summary>
    public interface IFeatureProvider
    {
        /// <summary>
        /// Gets the set of features.
        /// </summary>
        IReadOnlyCollection<IFeature> Features { get;  }

        /// <summary>
        /// Gets the feature with the given feature id.
        /// </summary>
        /// <param name="feaureId">The feature id.</param>
        /// <returns>The feature instance.</returns>
        IFeature GetFeature(FeatureId feaureId);
    }
}
