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
        /// Gets the current culture.
        /// </summary>
        public CultureInfo Culture => CultureInfo.CurrentUICulture;
    }
}
