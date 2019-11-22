// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a collection of work context extensions.
    /// </summary>
    public class WorkContextExtensionCollection : IWorkContextExtensionCollection
    {
        private static readonly KeyComparer _comparer = new KeyComparer();
        private readonly IWorkContextExtensionCollection? _defaults;
        private IDictionary<Type, object>? _extensions;
        private volatile int _revision;

        /// <summary>
        /// Initialises a new instance of <see cref="WorkContextExtensionCollection"/>.
        /// </summary>
        /// <param name="defaults">[Optional] The set of default extensions/</param>
        public WorkContextExtensionCollection(IWorkContextExtensionCollection? defaults = null)
        {
            _defaults = defaults;
        }

        /// <inheritdoc />
        public virtual int Revision
            => _revision + (_defaults?.Revision ?? 0);

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public object? this[Type key]
        {
            get
            {
                Ensure.IsNotNull(key, nameof(key));

                return _extensions is object
                    && _extensions.TryGetValue(key, out object result)
                    ? result
                    : _defaults?[key];
            }
            set
            {
                Ensure.IsNotNull(key, nameof(key));

                if (value is null)
                {
                    if (_extensions is object && _extensions.Remove(key))
                    {
                        _revision++;
                    }
                    return;
                }

                if (_extensions is null)
                {
                    _extensions = new Dictionary<Type, object>();
                }
                _extensions[key] = value;
                _revision++;
            }
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            if (_extensions is object)
            {
                foreach (var pair in _extensions)
                {
                    yield return pair;
                }
            }

            if (_defaults is object)
            {
                foreach (var pair in _extensions is null ? _defaults : _defaults.Except(_extensions, _comparer))
                {
                    yield return pair;
                }
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <summary>
        /// Provides services for comparing items in the collection for equality using the <see cref="Type"/> key.
        /// </summary>
        private class KeyComparer : IEqualityComparer<KeyValuePair<Type, object>>
        {
            /// <inheritdoc />
            public bool Equals(KeyValuePair<Type, object> x, KeyValuePair<Type, object> y)
                => x.Key.Equals(y.Key);

            /// <inheritdoc />
            public int GetHashCode(KeyValuePair<Type, object> obj)
                => obj.Key.GetHashCode();
        }
    }
}
