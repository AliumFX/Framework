// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a default implementation of an object comparer.
    /// </summary>
    public class DefaultObjectComparer<TObject> : IObjectComparer<TObject>
        where TObject : class
    {
        private readonly Func<TObject, object>[] _props;

        /// <summary>
        /// Initialises a new instance of <see cref="DefaultObjectComparer{TObject}"/>.
        /// </summary>
        /// <param name="props">The set of property getters.</param>
        public DefaultObjectComparer(params Func<TObject, object>[] props)
        {
            _props = props;

            LowValue = 17;
            HighValue = 397;
        }

        /// <summary>
        /// Gets or sets the high value.
        /// </summary>
        public virtual int HighValue { get; }

        /// <summary>
        /// Gets or sets the low value.
        /// </summary>
        public virtual int LowValue { get; }

        /// <inheritdoc />
        public virtual int Compare(TObject x, TObject y)
        {
            if (_props == null || _props.Length == 0)
            {
                // MA - We have nothing to compare.
                return 0;
            }

            if (x == null && y == null)
            {
                // MA - Both instances are null values.
                return 0;
            }

            if (ReferenceEquals(x, y))
            {
                // MA - Exact same reference.
                return 0;
            }

            if (x == null)
            {
                // MA - x is null, so that comes first.
                return -1;
            }

            if (y == null)
            {
                // MA - x is null, and y is not, so y comes first.
                return 1;
            }

            // MA - Create a default object comparer.
            var comparer = Comparer<object>.Default;

            foreach (var prop in _props)
            {
                var result = comparer.Compare(prop(x), prop(y));
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        /// <inheritdoc />
        public virtual bool Equals(TObject x, TObject y)
        {
            if (ReferenceEquals(x, y))
            {
                // MA - Same instances.
                return true;
            }

            if (x == null || y == null)
            {
                // MA - One of the items is null.
                return false;
            }

            if (_props == null || _props.Length == 0)
            {
                // MA - Nothing to compare.
                return true;
            }

            foreach (var prop in _props)
            {
                if (!Equals(prop(x), prop(y)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public virtual int GetHashCode(TObject obj)
        {
            if (_props == null || _props.Length == 0 || obj == null)
            {
                // MA - Nothing to generate hashcode for.
                return 0;
            }

            unchecked
            {
                int hash = LowValue;

                foreach (var prop in _props)
                {
                    object value = prop(obj);

                    if (value == null)
                    {
                        hash = hash * HighValue - 1;
                    }
                    else
                    {
                        hash = (hash * HighValue) ^ value.GetHashCode();
                    }
                }

                return hash;
            }
        }
    }
}
