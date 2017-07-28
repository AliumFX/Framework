// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides extensions for the <see cref="IEnumerable" /> type.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Orders the given set of items by dependencies.
        /// </summary>
        /// <typeparam name="TElement">The element type.</typeparam>
        /// <typeparam name="TDependencyKey">The dependency key type.</typeparam>
        /// <param name="source">The source set of items.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="dependentSelector">The dependent key selector.</param>
        /// <returns>The ordered set of items.</returns>
        public static IEnumerable<TElement> OrderByDependencies<TElement, TDependencyKey>(
            this IEnumerable<TElement> source, 
            Func<TElement, TDependencyKey> keySelector, 
            Func<TElement, TDependencyKey, IEnumerable<TDependencyKey>> dependentSelector)
            => new DependencyKeyOrderedEnumerable<TElement, TDependencyKey>(source, keySelector, dependentSelector);
    }
}
