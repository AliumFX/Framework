// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="TaskExecutor"/> type.
    /// </summary>
    public class TaskExecutorTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
            Assert.Throws<ArgumentNullException>("scopeFactory", () =>
                new TaskExecutor(null /* scopeFactory */));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        }

        [Fact]
        public async Task ExecuteStartupTasksAsync_CreatesScopedServiceProvider()
        {
            // Arrange
            bool scopeCreated = false;
            var scopeFactory = CreateServiceScopeFactory<IStartupTask>(
                onCreateScope: () => scopeCreated = true);
            var executor = new TaskExecutor(scopeFactory);

            // Act
            await executor.ExecuteStartupTasksAsync();

            // Assert
            Assert.True(scopeCreated);
        }

        [Fact]
        public async Task ExecuteStartupTasksAsync_ExecutesStartupTasks()
        {
            // Arrange
            bool taskExecuted = false;
            var scopeFactory = CreateServiceScopeFactory<IStartupTask>(
                service: CreateStartupTask(
                    onStartup: () => taskExecuted = true));
            var executor = new TaskExecutor(scopeFactory);

            // Act
            await executor.ExecuteStartupTasksAsync();

            // Assert
            Assert.True(taskExecuted);
        }

        [Fact]
        public async Task ExecuteShutdownTasksAsync_CreatesScopedServiceProvider()
        {
            // Arrange
            bool scopeCreated = false;
            var scopeFactory = CreateServiceScopeFactory<IShutdownTask>(
                onCreateScope: () => scopeCreated = true);
            var executor = new TaskExecutor(scopeFactory);

            // Act
            await executor.ExecuteShutdownTasksAsync();

            // Assert
            Assert.True(scopeCreated);
        }

        [Fact]
        public async Task ExecuteShutdownTasksAsync_ExecutesStartupTasks()
        {
            // Arrange
            bool taskExecuted = false;
            var scopeFactory = CreateServiceScopeFactory<IShutdownTask>(
                service: CreateShutdownTask(
                    onShutdown: () => taskExecuted = true));
            var executor = new TaskExecutor(scopeFactory);

            // Act
            await executor.ExecuteShutdownTasksAsync();

            // Assert
            Assert.True(taskExecuted);
        }

        private IServiceScopeFactory CreateServiceScopeFactory<TService>(
            Action? onCreateScope = null,
            TService service = default(TService))
        {
            var mock = new Mock<IServiceScopeFactory>();

            mock.Setup(ssf => ssf.CreateScope())
                .Callback(() =>
                {
                    onCreateScope?.DynamicInvoke();
                })
                .Returns(() => CreateServiceScope<TService>(service));

            return mock.Object;
        }

        private IServiceScope CreateServiceScope<TService>(
            TService service = default(TService))
        {
            var mock = new Mock<IServiceScope>();

            mock.Setup(ss => ss.ServiceProvider)
                .Returns(() => CreateServiceProvider<TService>(service));

            return mock.Object;
        }

        private IServiceProvider CreateServiceProvider<TService>(
            TService service = default(TService))
        {
            var mock = new Mock<IServiceProvider>();

            mock.Setup(sp => sp.GetService(It.Is<Type>(t => t == typeof(IEnumerable<TService>))))
                .Returns(() =>
                {
#pragma warning disable CS8653 // A default expression introduces a null value when 'TService' is a non-nullable reference type.
                    if (Equals(default(TService), service))
#pragma warning restore CS8653 // A default expression introduces a null value when 'TService' is a non-nullable reference type.
                    {
                        return Array.Empty<TService>();
                    }

                    return new[] { service };
                });

            return mock.Object;
        }

        private IStartupTask CreateStartupTask(Action onStartup)
        {
            var mock = new Mock<IStartupTask>();

            mock.Setup(st => st.ExecuteAsync(It.IsAny<CancellationToken>()))
                .Callback(onStartup)
                .Returns(Task.CompletedTask);

            return mock.Object;
        }

        private IShutdownTask CreateShutdownTask(Action onShutdown)
        {
            var mock = new Mock<IShutdownTask>();

            mock.Setup(st => st.ExecuteAsync(It.IsAny<CancellationToken>()))
                .Callback(onShutdown)
                .Returns(Task.CompletedTask);

            return mock.Object;
        }
    }
}
