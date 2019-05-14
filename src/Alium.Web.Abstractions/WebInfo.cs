// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using Alium.Modules;

    /// <summary>
    /// Provides constants and static values related to the web module.
    /// </summary>
    public static class WebInfo
    {
        /// <summary>
        /// The web module description.
        /// </summary>
        public const string WebModuleDescription = "Provides base services for a web application";

        /// <summary>
        /// The web module id.
        /// </summary>
        public static readonly ModuleId WebModuleId = new ModuleId("Web");

        /// <summary>
        /// The web module name.
        /// </summary>
        public const string WebModuleName = "Web";
    }
}
