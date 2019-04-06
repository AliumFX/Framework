// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents a module id.
    /// </summary>
    [DebuggerDisplay("Module Id: {Value}")]
    public readonly struct ModuleId : IComparable<string?>, IComparable<ModuleId>, IEquatable<string?>, IEquatable<ModuleId>
    {
        /// <summary>
        /// Represents an empty module id.
        /// </summary>
        public static readonly ModuleId Empty = new ModuleId();

        /// <summary>
        /// Initialises a new instance of <see cref="ModuleId"/>.
        /// </summary>
        /// <param name="value">The module id value.</param>
        public ModuleId(string value)
        {
            HasValue = true;
            Value = Ensure.IsNotNullOrEmpty(value, nameof(value));
        }

        /// <summary>
        /// Gets whether the current instance has a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the module id value.
        /// </summary>
        public string Value { get; }

        /// <inheritdoc />
        public int CompareTo(string? other)
        {
            if (HasValue)
            {
                int stringCompare = StringComparer.OrdinalIgnoreCase.Compare(Value, other);
                return stringCompare < 0
                    ? -1
                    : stringCompare == 0
                        ? 0
                        : 1;
            }

            if (string.IsNullOrEmpty(other))
            {
                return 1;
            }

            return 0;
        }

        /// <inheritdoc />
        public int CompareTo(ModuleId other)
        {
            if (other.HasValue)
            {
                return CompareTo(other.Value);
            }

            if (HasValue)
            {
                return 1;
            }

            return 0;
        }

        /// <inheritdoc />
        public bool Equals(string? other)
            => CompareTo(other) == 0;

        /// <inheritdoc />
        public bool Equals(ModuleId other)
            => CompareTo(other) == 0;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is ModuleId c)
            {
                return Equals(c);
            }
            if (obj is string s)
            {
                return Equals(s);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HasValue ? Value.GetHashCode() : 0;

        /// <inheritdoc />
        public override string ToString()
            => Value;

        /// <summary>
        /// Converts from a <see cref="ModuleId"/> to a <see cref="string"/>
        /// </summary>
        /// <param name="moduleId">The <see cref="ModuleId"/> value.</param>
        public static explicit operator string(ModuleId moduleId)
            => moduleId.Value;

        /// <summary>
        /// Converts from a <see cref="string"/> to a <see cref="ModuleId"/>
        /// </summary>
        /// <param name="value">The <see cref="string"/> value.</param>
        public static explicit operator ModuleId(string value)
            => string.IsNullOrEmpty(value) ? Empty : new ModuleId(value);
    }
}
