// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;

    /// <summary>
    /// Provides a base implementation of a disposable object
    /// </summary>
    public abstract class DisposableBase : IDisposable
    {
        /// <summary>
        /// Gets whether the object is disposed
        /// </summary>
        public bool Disposed { get; private set; }

        /// <inheritdoc />
        public void Dispose()
            => Dispose(true);

        /// <summary>
        /// Releases resources used by this instance
        /// </summary>
        /// <param name="disposing">Flag to state whether we are explicity disposing of this instance</param>
        protected void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                Disposed = true;

                if (disposing)
                {
                    // MA - We are explicitly disposing of this instance, through calls to Dispose()
                    DisposeExplicit();
                }

                // MA - Implicit dispose may occur through calls to Dispose() and finalisation by the GC
                DisposeImplicit();

                // MA - Tell the GC not to finalise this object
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Performs operations when the instance is explicitly disposed
        /// </summary>
        protected virtual void DisposeExplicit() { }

        /// <summary>
        /// Performs operations when the instance is implicitly disposed
        /// </summary>
        protected virtual void DisposeImplicit() { }

        /// <summary>
        /// Ensures the instance is not disposed
        /// </summary>
        /// <remarks>
        /// This method is provided for implementors to check preconditions before allowing operations
        /// </remarks>
        /// <param name="objectName">[Optional] The object name</param>
        /// <param name="message">[Optional] The message to pass to the thrown exception</param>
        protected void EnsureNotDisposed(string objectName = null, string message = null)
        {
            if (Disposed)
            {
                objectName = objectName ?? GetType().Name;
                message = message ?? CoreAbstractionsResources.ObjectDisposedMessage;

                throw new ObjectDisposedException(objectName, message);
            }
        }

        /// <summary>
        /// Finalises the object instance
        /// </summary>
        ~DisposableBase()
        {
            Dispose(false);
        }
    }
}
