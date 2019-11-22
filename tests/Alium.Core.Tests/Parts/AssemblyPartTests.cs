// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Parts
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="AssemblyPart"/>.
    /// </summary>
    public class AssemblyPartTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("assembly", () => new AssemblyPart(null! /* assembly */));
        }

        [Fact]
        public void Name_Returns_AssemblyName()
        {
            // Arrange
            var part = new AssemblyPart(typeof(AssemblyPartTests).Assembly);

            // Act
            string name = part.Name;

            // Assert
            Assert.Equal("Alium.Core.Tests", name);
        }

        [Fact]
        public void Types_Returns_DefinedTypes()
        {
            // Arrange
            var assembly = typeof(AssemblyPartTests).Assembly;
            var part = new AssemblyPart(assembly);

            // Act
            var types = part.Types;

            // Assert
            Assert.Equal(assembly.DefinedTypes, types);
            Assert.NotSame(assembly.DefinedTypes, types);
        }

        [Fact]
        public void Assembly_Returns_Assembly()
        {
            // Arrange
            var assembly = typeof(AssemblyPartTests).Assembly;
            var part = new AssemblyPart(assembly);

            // Act
            var partAssembly = part.Assembly;

            // Assert
            Assert.Equal(part.Assembly, partAssembly);
        }

        /*[Fact]
        public void GetReferencePaths_ReturnsReferencesFromDependencyContext_IfPreserveCompilationContextIsSet()
        {
            // Arrange
            var assembly = GetType().GetTypeInfo().Assembly;
            var part = new AssemblyPart(assembly);

            // Act
            var references = part.GetReferencePaths().ToList();

            // Assert
            Assert.Contains(assembly.Location, references);
            Assert.Contains(
                typeof(AssemblyPart).GetTypeInfo().Assembly.GetName().Name, 
                references.Select(Path.GetFileNameWithoutExtension));
        }*/

        [Fact]
        public void GetReferencePaths_ReturnsAssemblyLocation_IfPreserveCompilationContextIsNotSet()
        {
            // Arrange
            var assembly = typeof(AssemblyPart).Assembly;
            var part = new AssemblyPart(assembly);

            // Act
            var references = part.GetReferencePaths().ToList();

            // Assert
            var actual = Assert.Single(references);
            Assert.Equal(assembly.Location, actual);
        }

        [Fact]
        public void GetReferencePaths_ReturnsEmptySequenceForDynamicAssembly()
        {
            // Assert
            var name = new AssemblyName($"DynamicAssembly-{Guid.NewGuid()}");
            var assembly = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndCollect);
            var part = new AssemblyPart(assembly);

            // Act
            var references = part.GetReferencePaths().ToList();

            // Assert
            Assert.Empty(references);
        }
    }
}
