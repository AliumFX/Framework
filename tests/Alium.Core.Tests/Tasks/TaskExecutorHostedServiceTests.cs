// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tasks
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="TaskExecutorHostedService"/> type.
    /// </summary>
    public class TaskExecutorHostedServiceTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>("executor", () =>
                new TaskExecutorHostedService(
                    null! /* executor */));
        }

        [Fact]
        public async Task StartAsync_ExecutesStartupTasks()
        {
            // Arrange
            bool startupExecuted = false;
            var service = new TaskExecutorHostedService(
                CreateTaskExecutor(onStartup: () => startupExecuted = true));

            // Act
            await service.StartAsync(CancellationToken.None);

            // Assert
            Assert.True(startupExecuted);
        }

        [Fact]
        public async Task StopAsync_ExecutesShutdownTasks()
        {
            // Arrange
            bool shutdownExecuted = false;
            var service = new TaskExecutorHostedService(
                CreateTaskExecutor(onShutdown: () => shutdownExecuted = true));

            // Act
            await service.StopAsync(CancellationToken.None);

            // Assert
            Assert.True(shutdownExecuted);
        }

        private ITaskExecutor CreateTaskExecutor(Action? onStartup = null, Action? onShutdown = null)
        {
            var mock = new Mock<ITaskExecutor>();

            mock.Setup(te => te.ExecuteStartupTasksAsync(It.IsAny<CancellationToken>()))
                .Returns(() =>
                {
                    onStartup?.DynamicInvoke();

                    return Task.CompletedTask;
                });

            mock.Setup(te => te.ExecuteShutdownTasksAsync(It.IsAny<CancellationToken>()))
                .Returns(() =>
                {
                    onShutdown?.DynamicInvoke();

                    return Task.CompletedTask;
                });

            return mock.Object;
        }
    }
}
