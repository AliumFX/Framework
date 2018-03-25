// Copyright (c) Alium Fx. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System.Globalization;

    /// <summary>
    /// Provides access to the current culture through the work context.
    /// </summary>
    public class CultureWorkContextExtension : ICultureWorkContextExtension
    {
        /// <summary>
        /// Initialises a new instance of <see cref="CultureWorkContextExtension"/>
        /// </summary>
        public CultureWorkContextExtension()
            : this(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture)
        { }

        /// <summary>
        /// Initialises a new instance of <see cref="CultureWorkContextExtension"/>
        /// </summary>
        /// <param name="formattingCulture">The formatting culture</param>
        /// <param name="resourceCulture">The resource culture</param>
        public CultureWorkContextExtension(CultureInfo formattingCulture, CultureInfo resourceCulture)
        {
            FormattingCulture = Ensure.IsNotNull(formattingCulture, nameof(formattingCulture));
            ResourceCulture = Ensure.IsNotNull(resourceCulture, nameof(resourceCulture));
        }

        /// <summary>
        /// Gets the current formatting culture
        /// </summary>
        public CultureInfo FormattingCulture { get; }

        /// <summary>
        /// Gets or sets the resource culture
        /// </summary>
        public CultureInfo ResourceCulture { get; }
    }
}
