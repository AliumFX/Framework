// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the required contract for implementing a work context feature collection.
    /// </summary>
    public interface IWorkContextExtensionCollection : IEnumerable<KeyValuePair<Type, object>>
    {
        /// <summary>
        /// Gets whethr the collection is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets the revision number.
        /// </summary>
        int Revision { get; }

        /// <summary>
        /// Gets or sets the feature with the given type.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object? this[Type key] { get; set; }
    }
}
