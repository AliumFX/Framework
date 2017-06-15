// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Modules
{
    using System;

    /// <summary>
    /// Represents a system code.
    /// </summary>
    public struct ModuleId : IComparable<string>, IComparable<ModuleId>, IEquatable<string>, IEquatable<ModuleId>
    {
        /// <summary>
        /// Represents an empty sys code.
        /// </summary>
        public static readonly ModuleId Empty = new ModuleId();

        /// <summary>
        /// Initialises a new instance of <see cref="ModuleId"/>.
        /// </summary>
        /// <param name="value">The system code value.</param>
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
        /// Gets the system code.
        /// </summary>
        public string Value { get; }

        /// <inheritdoc />
        public int CompareTo(string other)
        {
            if (HasValue)
            {
                return StringComparer.OrdinalIgnoreCase.Compare(Value, other);
            }

            if (string.IsNullOrEmpty(other))
            {
                return -1;
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
                return -1;
            }

            return 0;
        }

        /// <inheritdoc />
        public bool Equals(string other)
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
            => $"SysCode: {Value}";

        /// <summary>
        /// Converts from a <see cref="ModuleId"/> to a <see cref="string"/>
        /// </summary>
        /// <param name="sysCode">The <see cref="ModuleId"/> value.</param>
        public static explicit operator string(ModuleId sysCode)
            => sysCode.Value;

        /// <summary>
        /// Converts from a <see cref="string"/> to a <see cref="ModuleId"/>
        /// </summary>
        /// <param name="value">The <see cref="string"/> value.</param>
        public static explicit operator ModuleId(string value)
            => string.IsNullOrEmpty(value) ? Empty : new ModuleId(value);
    }
}
