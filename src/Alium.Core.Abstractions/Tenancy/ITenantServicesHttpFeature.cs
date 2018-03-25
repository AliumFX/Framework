// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;

    /// <summary>
    /// Defines the required contract for implementing a HTTP feature to access tenant services
    /// </summary>
    public interface ITenantServicesHttpFeature
    {
        /// <summary>
        /// Gets the tenant services
        /// </summary>
        IServiceProvider TenantServices { get; }
    }
}
