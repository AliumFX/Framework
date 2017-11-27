// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System.Globalization;

    using Alium.Tenancy;

    /// <summary>
    /// Represents a context for performing work.
    /// </summary>
    public class WorkContext : IWorkContext
    {
        private WorkContextExtensionReferences<StandardExtensions> _extensions;

        private ICultureWorkContextExtension _cultureFeature => _extensions.Fetch(ref _extensions.Cache.Culture, e => new CultureWorkContextExtension());
        private ITenantWorkContextExtension _tenantFeature => _extensions.Fetch(ref _extensions.Cache.Tenant, e => new TenantWorkContextExtension(TenantId.Default));

        /// <summary>
        /// Initialises a new instance of <see cref="WorkContext"/>
        /// </summary>
        public WorkContext()
            : this(new WorkContextExtensionCollection())
        {

        }

        /// <summary>
        /// Initialises a new instance of <see cref="WorkContext"/>.
        /// </summary>
        /// <param name="extensions"></param>
        public WorkContext(IWorkContextExtensionCollection extensions)
        {
            _extensions = new WorkContextExtensionReferences<StandardExtensions>(extensions);
        }

        /// <inheritdoc />
        public CultureInfo Culture => _cultureFeature.Culture;

        /// <inheritdoc />
        public IWorkContextExtensionCollection Extensions => _extensions.Extensions;

        /// <inheritdoc />
        public TenantId TenantId => _tenantFeature.TenantId;

        /// <summary>
        /// A caching object for storing standard extension references.
        /// </summary>
        private struct StandardExtensions
        {
            /// <summary>
            /// The standard culture extension.
            /// </summary>
            public ICultureWorkContextExtension Culture;

            /// <summary>
            /// The standard tenant extension.
            /// </summary>
            public ITenantWorkContextExtension Tenant;
        }
    }
}
