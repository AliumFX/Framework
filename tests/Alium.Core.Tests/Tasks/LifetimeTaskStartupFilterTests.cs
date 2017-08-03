// Copyright (c) Alium FX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Alium.Tasks
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Moq;
    using Xunit;

    /// <summary>
    /// Provides tests for the <see cref="TaskExecutorStartupFilter"/> type.
    /// </summary>
    public class LifetimeTaskStartupFilterTests
    {
        [Fact]
        public void Constructor_ValidatesArguments()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                new TaskExecutorStartupFilter(
                    null /* executor */,
                    null /* applicationLifetime */));

            Assert.Throws<ArgumentNullException>(() =>
                new TaskExecutorStartupFilter(
                    Mock.Of<ITaskExecutor>(),
                    null /* applicationLifetime */));
        }

        [Fact]
        public void Configure_ValidatesArguments()
        {
            // Arrange
            var filter = new TaskExecutorStartupFilter(
                CreateTaskExecutor(),
                CreateApplicationLifetime());

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => filter.Configure(null /* next */));
        }

        [Fact]
        public void Configure_ExecutesStartupTasks()
        {
            // Arrange
            bool startupExecuted = false;
            var filter = new TaskExecutorStartupFilter(
                CreateTaskExecutor(onStartup: () => startupExecuted = true),
                CreateApplicationLifetime());

            Action<IApplicationBuilder> configure = _ => { };

            // Act
            filter.Configure(configure);

            // Assert
            Assert.True(startupExecuted);
        }

        [Fact]
        public void Configure_RegistersWitHCancellationToken_ToExecuteShutdownTasks()
        {
            // Arrange
            bool shutdownExecuted = false;
            var cts = new CancellationTokenSource();
            var filter = new TaskExecutorStartupFilter(
                CreateTaskExecutor(onShutdown: () => shutdownExecuted = true),
                CreateApplicationLifetime(stopped: cts.Token));

            Action<IApplicationBuilder> configure = _ => { };

            // Act
            filter.Configure(configure);
            cts.Cancel();

            // Assert
            Assert.True(shutdownExecuted);
        }

        private ITaskExecutor CreateTaskExecutor(Action onStartup = null, Action onShutdown = null)
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

        private IApplicationLifetime CreateApplicationLifetime(
            CancellationToken started = default(CancellationToken),
            CancellationToken stopped = default(CancellationToken),
            CancellationToken stopping = default(CancellationToken))
        {
            var mock = new Mock<IApplicationLifetime>();

            mock.Setup(al => al.ApplicationStarted)
                .Returns(started);
            mock.Setup(al => al.ApplicationStopped)
                .Returns(stopped);
            mock.Setup(al => al.ApplicationStopping)
                .Returns(stopping);

            return mock.Object;
        }
    }
}
