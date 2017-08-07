// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the required contract for implementing a features builder.
    /// </summary>
    public interface IFeaturesBuilder
    {
        /// <summary>
        /// Builds the set of features.
        /// </summary>
        /// <param name="features">The features collection.</param>
        void BuildFeatures(ICollection<IFeature> features);
    }
}
