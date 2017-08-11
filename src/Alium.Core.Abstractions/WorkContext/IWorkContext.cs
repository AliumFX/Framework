// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    /// <summary>
    /// Defines the required contract for implementing a work context.
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets the set of work context extensions.
        /// </summary>
        IWorkContextExtensionCollection Extensions { get; }
    }
}
