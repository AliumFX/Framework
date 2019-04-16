// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Data
{
    /// <summary>
    /// Defines the required contract for implementing an entity
    /// </summary>
    /// <typeparam name="TPrimaryKey">The entity key type</typeparam>
    public interface IEntity<TPrimaryKey> 
        where TPrimaryKey : struct
    {
        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}
