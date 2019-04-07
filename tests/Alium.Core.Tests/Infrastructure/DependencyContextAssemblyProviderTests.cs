// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Core.Tests
{
    using System;
    using Microsoft.Extensions.DependencyModel;
    using Xunit;

    using Alium.Infrastructure;

    /// <summary>
    /// Provides tests for the <see cref="DependencyContextAssemblyProvider"/>
    /// </summary>
    public class DependencyContextAssemblyProviderTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>("dependencyContext", () => new DependencyContextAssemblyProvider(null /* dependencyContext */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public void Assemblies_FindsCandidateAssemblies()
        {
            // Arrange
            var assemblies = new[]
            {
                GetType().Assembly,
                typeof(DependencyContextAssemblyProvider).Assembly,
                typeof(Ensure).Assembly
            };
            var dependencyContext = DependencyContext.Load(assemblies[0]);
            var provider = new DependencyContextAssemblyProvider(dependencyContext);

            // Act
            var discoveredAssemblies = provider.Assemblies;

            // Assert
            Assert.NotNull(discoveredAssemblies);
            Assert.Equal(3, discoveredAssemblies.Count);
            Assert.Contains(discoveredAssemblies, a => a.FullName.Equals(assemblies[0].FullName));
            Assert.Contains(discoveredAssemblies, a => a.FullName.Equals(assemblies[1].FullName));
            Assert.Contains(discoveredAssemblies, a => a.FullName.Equals(assemblies[2].FullName));
        }
    }
}
