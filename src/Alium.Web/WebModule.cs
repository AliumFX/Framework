// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Web
{
    using System.Collections.Generic;

    using Alium.Features;
    using Alium.Modules;
    using Alium.Tenancy;
    
    /// <summary>
    /// Web module.
    /// </summary>
    public class WebModule : ModuleBase, IFeaturesBuilder
    {
        /// <summary>
        /// Initialises a new instance of <see cref="WebModule"/>.
        /// </summary>
        public WebModule()
            : base(WebInfo.WebModuleId, WebInfo.WebModuleName, WebInfo.WebModuleDescription,
                  dependencies: new[] { CoreInfo.CoreModuleId })
        { }

        /// <inheritdoc />
        public void BuildFeatures(ICollection<IFeature> features)
        {
            features.Add(new HttpTenancyFeature());
        }
    }
}
