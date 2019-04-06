// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Defines the required contract for implementing a configuration extender.
    /// </summary>
    public interface IAppConfigurationExtender
    {
        /// <summary>
        /// Extends the configuration by utilising the given builder.
        /// </summary>
        /// <param name="context">The host builder context.</param>
        /// <param name="builder">The configuration builder.</param>
        void BuildConfiguration(HostBuilderContext context, IConfigurationBuilder builder);
    }
}
