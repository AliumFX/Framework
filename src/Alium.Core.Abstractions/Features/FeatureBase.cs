// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Features
{
    using System;

    /// <summary>
    /// Provides a base implementation of a feature.
    /// </summary>
    public abstract class FeatureBase : IFeature
    {
        /// <summary>
        /// Initialises a new instance of <see cref="FeatureBase"/>.
        /// </summary>
        /// <param name="id">The feature id.</param>
        /// <param name="name">[Optional] The feature name.</param>
        /// <param name="description">[Optional] The feature description.</param>
        /// <param name="enabledByDefault">[Optional] Default enabled state.</param>
        protected FeatureBase(FeatureId id, string name = null, string description = null, bool enabledByDefault = false)
        {
            if (id.Equals(FeatureId.Empty))
            {
                throw new ArgumentException("The feature id value must be provided", nameof(id));
            }

            Id = id;
            Name = name;
            Description = description;
            EnabledByDefault = enabledByDefault;
        }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public bool EnabledByDefault { get; }

        /// <inheritdoc />
        public FeatureId Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public virtual void Initialise(FeatureInitialisationContext context)
        {

        }
    }
}
