// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Parts
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the required contract for implementing a compilation reference provider.
    /// </summary>
    public interface ICompilationReferenceProvider
    {
        /// <summary>
        /// Gets the reference paths used to perform runtime compilation.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetReferencePaths();
    }
}
