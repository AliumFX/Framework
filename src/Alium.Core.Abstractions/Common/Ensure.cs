// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium
{
    using System;

    /// <summary>
    /// Provides argument validation methods.
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Ensures the given argument is not null.
        /// </summary>
        /// <typeparam name="TArgument">The argument type.</typeparam>
        /// <param name="argument">The argument value.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <exception cref="ArgumentException">If the argument value is null or an empty string.</exception>
        /// <returns>The argument value.</returns>
        public static TArgument IsNotNull<TArgument>(TArgument argument, string parameterName)
            where TArgument : class
            => argument ?? throw new ArgumentNullException(parameterName);

        /// <summary>
        /// Ensures the given argument is not null or an empty string.
        /// </summary>
        /// <param name="argument">The argument value.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <exception cref="ArgumentException">If the argument value is null or an empty string.</exception>
        /// <returns>The argument value.</returns>
        public static string IsNotNullOrEmpty(string argument, string parameterName)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException($"The parameter '{parameterName}' cannot be null or an empty string.", parameterName);
            }

            return argument;
        }
    }
}
