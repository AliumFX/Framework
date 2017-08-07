// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;

    /// <summary>
    /// Defines the required contract for implementing a feature factory.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    public interface IFeatureFactory<TService>
    {
        /// <summary>
        /// Creates the feature with given feature id.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="featureId">The feature id.</param>
        /// <returns>The feature service.</returns>
        IFeature<TService> CreateFeature(IServiceProvider serviceProvider, FeatureId featureId);
    }
}
