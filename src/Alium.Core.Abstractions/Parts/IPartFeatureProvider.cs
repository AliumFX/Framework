// Copyright (c) Alium Project. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Parts
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the required contract for implementing a part feature provider.
    /// </summary>
    public interface IPartFeatureProvider
    {
    }

    /// <summary>
    /// Defines the required contract for implementing a part feature provider.
    /// </summary>
    /// <typeparam name="TPart">The part type.</typeparam>
    public interface IPartFeatureProvider<TPartFeature> : IPartFeatureProvider
        where TPartFeature : class
    {
        /// <summary>
        /// Updates the part feature with compatible parts.
        /// </summary>
        /// <param name="parts">The set of parts.</param>
        /// <param name="feature">The part feature.</param>
        void PopulateFeature(IEnumerable<IPart> parts, TPartFeature feature);
    }
}
