// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System.Globalization;

    using Alium.Tenancy;

    /// <summary>
    /// Defines the required contract for implementing a work context.
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets the set of work context extensions
        /// </summary>
        IWorkContextExtensionCollection Extensions { get; }

        /// <summary>
        /// Gets the formatting culture
        /// </summary>
        CultureInfo FormattingCulture { get; }

        /// <summary>
        /// Gets the resource culture
        /// </summary>
        CultureInfo ResourceCulture { get; }

        /// <summary>
        /// Gets the current tenant id
        /// </summary>
        TenantId TenantId { get; }


    }
}
