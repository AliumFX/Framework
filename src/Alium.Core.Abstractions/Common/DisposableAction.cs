// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;

    /// <summary>
    /// Provides a mechanism through which an action can be executed when the object is disposed
    /// </summary>
    public class DisposableAction : DisposableBase
    {
        private readonly Action _action;
        private readonly bool _allowOnImplicitDispose;

        /// <summary>
        /// Initialises a new instance of <see cref="DisposableAction"/>
        /// </summary>
        /// <param name="action">The disposing action</param>
        /// <param name="allowOnImplictDispose">[Optional] Allow the dispose action to execute when implicitly disposing of the instance (default = false)</param>
        public DisposableAction(Action action, bool allowOnImplictDispose = false)
        {
            _action = Ensure.IsNotNull(action, nameof(action));
            _allowOnImplicitDispose = allowOnImplictDispose;
        }

        /// <inheritdoc />
        protected override void DisposeExplicit()
        {
            if (!_allowOnImplicitDispose)
            {
                _action();
            }
        }

        /// <inheritdoc />
        protected override void DisposeImplicit()
        {
            if (_allowOnImplicitDispose)
            {
                _action();
            }
        }
    }

    /// <summary>
    /// Provides a mechanism through which an action can be executed when the object is disposed
    /// </summary>
    /// <typeparam name="T">The disposable action type</typeparam>
    public class DisposableAction<T> : DisposableBase
        where T : DisposableAction<T>
    {
        private readonly Action<T> _action;
        private readonly bool _allowOnImplicitDispose;

        /// <summary>
        /// Initialises a new instance of <see cref="DisposableAction"/>
        /// </summary>
        /// <param name="action">The disposing action</param>
        /// <param name="allowOnImplictDispose">[Optional] Allow the dispose action to execute when implicitly disposing of the instance (default = false)</param>
        public DisposableAction(Action<T> action, bool allowOnImplictDispose = false)
        {
            _action = Ensure.IsNotNull(action, nameof(action));
            _allowOnImplicitDispose = allowOnImplictDispose;
        }

        /// <inheritdoc />
        protected override void DisposeExplicit()
        {
            if (!_allowOnImplicitDispose)
            {
                _action((T)this);
            }
        }

        /// <inheritdoc />
        protected override void DisposeImplicit()
        {
            if (_allowOnImplicitDispose)
            {
                _action((T)this);
            }
        }
    }
}
