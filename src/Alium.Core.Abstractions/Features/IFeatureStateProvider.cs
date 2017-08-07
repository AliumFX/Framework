// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    /// <summary>
    /// Defines the required contract for implementing a feature state provider.
    /// </summary>
    public interface IFeatureStateProvider
    {
        /// <summary>
        /// Gets the feature state for the given feature id.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <returns>The feature state.</returns>
        FeatureState GetFeatureState(FeatureId featureId);
    }
}
