// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Data
{
    using System;

    using Alium.Administration;

    /// <summary>
    /// Provides a default implementation of an entity using an <see cref="int" /> primary key
    /// </summary>
    public abstract class Entity : Entity<int>, IEntity
    {
    }

    /// Provides a default implementation of an entity using a custom primary key type
    /// </summary>
    public abstract class Entity<TPrimaryKey> : Entity<TPrimaryKey, AdministrationUserId>, IEntity<TPrimaryKey>
        where TPrimaryKey : struct
    { }

    /// <summary>
    /// Provides a default implementation of an entity using a custom primary key type
    /// </summary>
    public abstract class Entity<TPrimaryKey, TUserId> : IEntity<TPrimaryKey, TUserId>
        where TPrimaryKey : struct
        where TUserId: struct
    {
        /// <summary>
        /// Gets or sets the date/time (with offset to UTC) the entity was created
        /// </summary>
        public DateTimeOffset CreatedDateTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user that created the entity
        /// </summary>
        public TUserId CreatedUserId { get; set; }

        /// <summary>
        /// Gets or sets whether the entity is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets whether the entity is deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets whether the entity is hidden
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public TPrimaryKey Id { get; set; }

        /// <summary>
        /// Gets or sets whether the entity is read-only
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the date/time (with offset to UTC) the entity was created
        /// </summary>
        public DateTimeOffset? UpdatedDateTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user that created the entity
        /// </summary>
        public TUserId? UpdatedUserId { get; set; }
    }
}
