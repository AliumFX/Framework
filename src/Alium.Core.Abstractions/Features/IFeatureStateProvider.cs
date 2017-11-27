// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using Alium.Tenancy;

    /// <summary>
    /// Defines the required contract for implementing a feature state provider.
    /// </summary>
    public interface IFeatureStateProvider
    {
        /// <summary>
        /// Sets a tenant scope when resolving feature states.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        void BeginTenantScope(TenantId tenantId);

        /// <summary>
        /// Gets the feature state for the given feature id.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <returns>The feature state.</returns>
        FeatureState GetFeatureState(FeatureId featureId);

        /// <summary>
        /// Gets the feature state for the given feature id.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="tenantId">The tenant id.</param>
        /// <returns>The feature state.</returns>
        FeatureState GetFeatureState(FeatureId featureId, TenantId tenantId);
    }
}
