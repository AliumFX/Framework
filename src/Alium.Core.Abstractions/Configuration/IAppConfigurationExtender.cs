// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Configuration
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Defines the required contract for implementing a configuration extender.
    /// </summary>
    public interface IAppConfigurationExtender
    {
        /// <summary>
        /// Extends the configuration by utilising the given builder.
        /// </summary>
        /// <param name="context">The web host builder context.</param>
        /// <param name="builder">The configuration builder.</param>
        void BuildConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder);
    }
}
