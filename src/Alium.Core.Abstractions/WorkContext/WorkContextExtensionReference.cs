// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    /// <summary>
    /// Represents a reference to a work context extension.
    /// </summary>
    public struct WorkContextExtensionReference<TExtension>
        where TExtension : class
    {
        private TExtension? _extension;
        private int _revision;

        /// <summary>
        /// Gets the extension.
        /// </summary>
        public TExtension? Extension => _extension;

        /// <summary>
        /// Gets the revision number.
        /// </summary>
        public int Revision => _revision;

        /// <summary>
        /// Initialises a new instance of <see cref="WorkContextExtensionReference{TExtension}"/>.
        /// </summary>
        /// <param name="extension">The extension instance.</param>
        /// <param name="revision">The revision.</param>
        private WorkContextExtensionReference(TExtension extension, int revision)
        {
            _extension = Ensure.IsNotNull(extension, nameof(extension));
            _revision = revision;
        }

        /// <summary>
        /// Fetches the most recent reivion of the given extension.
        /// </summary>
        /// <param name="extensions">The extension instance.</param>
        /// <returns>The extension instance.</returns>
        public TExtension? Fetch(IWorkContextExtensionCollection extensions)
        {
            Ensure.IsNotNull(extensions, nameof(extensions));

            if (_revision == extensions.Revision)
            {
                return _extension;
            }

            _extension = extensions.GetExtension<TExtension>();
            _revision = extensions.Revision;

            return _extension;
        }

        /// <summary>
        /// Updates the extension reference.
        /// </summary>
        /// <param name="extensions">The set of extensions.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>The extension instance.</returns>
        public TExtension Update(IWorkContextExtensionCollection extensions, TExtension extension)
        {
            Ensure.IsNotNull(extensions, nameof(extensions));
            Ensure.IsNotNull(extension, nameof(extension));

            extensions.SetExtension<TExtension>(extension);
            _extension = extension;
            _revision = extensions.Revision;

            return extension;
        }
    }
}
