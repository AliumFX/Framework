// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Defines the required contract for implementing a services builder.
    /// </summary>
    public interface IServicesBuilder
    {
        /// <summary>
        /// Builds the services for the application.
        /// </summary>
        /// <param name="services">The set of services.</param>
        void BuildServices(IServiceCollection services);
    }
}
