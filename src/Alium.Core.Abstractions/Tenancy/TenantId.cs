// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tenancy
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents a tenant id.
    /// </summary>
    [DebuggerDisplay("Tenant Id: {Value}")]
    public readonly struct TenantId : IComparable<string?>, IComparable<TenantId>, IEquatable<string?>, IEquatable<TenantId>
    {
        /// <summary>
        /// Represents the defualt tenant id.
        /// </summary>
        public static readonly TenantId Default = new TenantId("default");

        /// <summary>
        /// Represents an empty tenant id.
        /// </summary>
        public static readonly TenantId Empty = new TenantId();

        /// <summary>
        /// Initialises a new instance of <see cref="TenantId"/>.
        /// </summary>
        /// <param name="value">The tenant id value.</param>
        public TenantId(string value)
        {
            HasValue = true;
            Value = Ensure.IsNotNullOrEmpty(value, nameof(value));
        }

        /// <summary>
        /// Gets whether the current instance has a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the tenant id value.
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
        public int CompareTo(TenantId other)
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
        public bool Equals(TenantId other)
            => CompareTo(other) == 0;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is TenantId t)
            {
                return Equals(t);
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
        /// Converts from a <see cref="TenantId"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="tenantId">The <see cref="TenantId"/>.</param>
        public static explicit operator string(TenantId tenantId)
            => tenantId.Value;

        /// <summary>
        /// Converts from a <see cref="string"/> to a <see cref="TenantId"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value.</param>
        public static explicit operator TenantId(string value)
            => string.IsNullOrEmpty(value) ? Empty : new TenantId(value);
    }
}
