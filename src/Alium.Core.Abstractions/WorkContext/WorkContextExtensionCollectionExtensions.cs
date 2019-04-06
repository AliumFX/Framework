// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    /// <summary>
    /// Provides extensions for the <see cref="IWorkContextExtensionCollection"/> type.
    /// </summary>
    public static class WorkContextExtensionCollectionExtensions
    {
        /// <summary>
        /// Gets the extension of the given type from the collection.
        /// </summary>
        /// <typeparam name="TExtension">The extension type.</typeparam>
        /// <param name="collection">The extension collection.</param>
        /// <returns>The extension instance.</returns>
        public static TExtension? GetExtension<TExtension>(this IWorkContextExtensionCollection collection)
            where TExtension: class
        {
            Ensure.IsNotNull(collection, nameof(collection));

            return collection[typeof(TExtension)] as TExtension;
        }

        /// <summary>
        /// Sets the extension on the collection.
        /// </summary>
        /// <typeparam name="TExtension">The extension type.</typeparam>
        /// <param name="collection">The extension collection.</param>
        /// <param name="extension">The extension instance.</param>
        public static void SetExtension<TExtension>(this IWorkContextExtensionCollection collection, TExtension extension)
            where TExtension: class
        {
            Ensure.IsNotNull(collection, nameof(collection));

            collection[typeof(TExtension)] = extension;
        }
    }
}
