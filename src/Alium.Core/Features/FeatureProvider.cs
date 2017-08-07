// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Provides the collection of features.
    /// </summary>
    public class FeatureProvider : IFeatureProvider
    {
        /// <summary>
        /// Initialises a new instance of <see cref="FeatureProvider"/>.
        /// </summary>
        /// <param name="features">The set of features.</param>
        public FeatureProvider(IEnumerable<IFeature> features)
        {
            Features = new ReadOnlyCollection<IFeature>(
                Ensure.IsNotNull(features, nameof(features)).ToList());
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IFeature> Features { get; }

        /// <inheritdoc />
        public IFeature GetFeature(FeatureId featureId)
            => Features.FirstOrDefault(f => featureId.Equals(f.Id));
    }
}
