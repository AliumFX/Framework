// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;

    /// <summary>
    /// Provides a cached lookup of a work context extension.
    /// </summary>
    public struct WorkContextExtensionReferences<TCache>
        where TCache : struct
    {
        /// <summary>
        /// Initialises a new instance of <see cref="WorkContextExtensionReferences{TCache}"/>.
        /// </summary>
        /// <param name="extensions">The set of extensions.</param>
        public WorkContextExtensionReferences(IWorkContextExtensionCollection extensions)
        {
            Extensions = Ensure.IsNotNull(extensions, nameof(extensions));
            Cache = default;
            Revision = Extensions.Revision;
        }

        public TCache Cache;

        /// <summary>
        /// Gets the collection of extensions.
        /// </summary>
        public IWorkContextExtensionCollection Extensions { get; private set; }

        /// <summary>
        /// Gets the collection revision.
        /// </summary>
        public int Revision { get; private set; }

        /// <summary>
        /// Fetches the given extension.
        /// </summary>
        /// <typeparam name="TExtension">The extension type.</typeparam>
        /// <typeparam name="TState">The state type.</typeparam>
        /// <param name="cached">The cached extension.</param>
        /// <param name="state">The state.</param>
        /// <param name="factory">The factory instance.</param>
        /// <returns>The extension instance.</returns>
        public TExtension Fetch<TExtension, TState>(ref TExtension cached, TState state, Func<TState, TExtension> factory)
            where TExtension : class
        {
            Ensure.IsNotNull(factory, nameof(factory));

            bool cleared = false;
            if (Revision != Extensions.Revision)
            {
                cleared = true;
                Cache = default;
                Revision = Extensions.Revision;
            }

            TExtension? extension = cached;
            if (extension == null || cleared)
            {
                extension = Extensions.GetExtension<TExtension>();
                if (extension == null)
                {
                    extension = factory(state);
                    Extensions.SetExtension(extension);
                    Revision = Extensions.Revision;
                }
                cached = extension;
            }

            return extension;
        }

        /// <summary>
        /// Fetches the given extension.
        /// </summary>
        /// <typeparam name="TExtension">The extension type.</typeparam>
        /// <param name="cached">The cached extension.</param>
        /// <param name="factory">The factory instance.</param>
        /// <returns>The extension instance.</returns>
        public TExtension Fetch<TExtension>(ref TExtension cached, Func<IWorkContextExtensionCollection, TExtension> factory)
            where TExtension : class
            => Fetch(ref cached, Extensions, factory);
    }
}
