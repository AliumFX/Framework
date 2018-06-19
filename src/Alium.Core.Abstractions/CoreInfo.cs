// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using Alium.Features;
    using Alium.Modules;

    /// <summary>
    /// Provides constants and static values related to the core module.
    /// </summary>
    public static class CoreInfo
    {
        /// <summary>
        /// The core module description.
        /// </summary>
        public const string CoreModuleDescription = "Provides core services for an application";

        /// <summary>
        /// The core module id.
        /// </summary>
        public static readonly ModuleId CoreModuleId = new ModuleId("Core");

        /// <summary>
        /// The core module name.
        /// </summary>
        public const string CoreModuleName = "Core";

        /// <summary>
        /// The features configuration file.
        /// </summary>
        public const string FeaturesConfigurationFile = "./config/features.json";

        /// <summary>
        /// The flags configuration file.
        /// </summary>
        public const string FlagsConfigurationFile = "./config/flags.json";

        /// <summary>
        /// The tenancy feature id.
        /// </summary>
        public static readonly FeatureId TenancyFeatureId = new FeatureId(CoreModuleId, "Tenancy");

        /// <summary>
        /// The events feature id.
        /// </summary>
        public static readonly FeatureId EventsFeatureId = new FeatureId(CoreModuleId, "Events");
    }
}
