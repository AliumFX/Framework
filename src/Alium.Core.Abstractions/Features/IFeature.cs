// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    /// <summary>
    /// Defines the required contract for implementing a feaure.
    /// </summary>
    public interface IFeature
    {
        /// <summary>
        /// Gets the feature id.
        /// </summary>
        FeatureId Id { get; }

        /// <summary>
        /// Gets the description of the feature.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the name of the feature.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the parent feature.
        /// </summary>
        IFeature ParentFeature { get; }

        /// <summary>
        /// Initialises the feature if the feature is enabled.
        /// </summary>
        void Initialise();
    }
}
