using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Alium.Data.ConsoleSample
{
    using Alium.Modules;
    using Microsoft.Extensions.Hosting;

    class Program
    {
        public static async Task Main(string[] args)
        {
            await BuildHost(args).RunConsoleAsync();
        }

        public static IHostBuilder BuildHost(string[] args) =>
            Host.CreateDefaultBuilder()
                .UseDiscoveredModules()
                .ConfigureLogging(lb => lb
                    .SetMinimumLevel(LogLevel.Trace)
                );
    }
}
