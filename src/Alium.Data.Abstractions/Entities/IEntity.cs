// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Data
{
    using System;

    using Alium.Administration;

    /// <summary>
    /// Defines the required contract for implementing an entity
    /// </summary>
    public interface IEntity : IEntity<int>
    {

    }

    /// <summary>
    /// Defines the required contract for implementing an entity
    /// </summary>
    /// <typeparam name="TPrimaryKey">The entity key type</typeparam>
    public interface IEntity<TPrimaryKey> : IEntity<TPrimaryKey, AdministrationUserId>
        where TPrimaryKey : struct
    {
    }

    /// <summary>
    /// Defines the required contract for implementing an entity
    /// </summary>
    /// <typeparam name="TPrimaryKey">The entity key type</typeparam>
    /// <typeparam name="TUserId">The user ID type</typeparam>
    public interface IEntity<TPrimaryKey, TUserId>
        where TPrimaryKey : struct
        where TUserId : struct
    {
        /// <summary>
        /// Gets or sets the date/time (with offset to UTC) the entity was created
        /// </summary>
        DateTimeOffset CreatedDateTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user that created the entity
        /// </summary>
        TUserId CreatedUserId { get; set; }

        /// <summary>
        /// Gets or sets whether the entity is enabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets whether the entity is deleted
        /// </summary>
        bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets whether the entity is hidden
        /// </summary>
        bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        /// Gets or sets whether the entity is read-only
        /// </summary>
        bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the date/time (with offset to UTC) the entity was created
        /// </summary>
        DateTimeOffset? UpdatedDateTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user that created the entity
        /// </summary>
        TUserId? UpdatedUserId { get; set; }
    }
}
