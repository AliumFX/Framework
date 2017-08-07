// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;

    /// <summary>
    /// Provides a context for initialising a feature.
    /// </summary>
    public class FeatureInitialisationContext
    {
        /// <summary>
        /// Initialises a new instance of <see cref="FeatureInitialisationContext"/>.
        /// </summary>
        /// <param name="applicationServices">The application service provider.</param>
        public FeatureInitialisationContext(IServiceProvider applicationServices)
        {
            ApplicationServices = Ensure.IsNotNull(applicationServices, nameof(applicationServices));
        }

        /// <summary>
        /// Gets the application service provider.
        /// </summary>
        public IServiceProvider ApplicationServices { get; }
    }
}
