using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Alium.Core.WebSample
{
    using Alium.Modules;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            await BuildHost(args).RunAsync();
        }

        public static IHost BuildHost(string[] args) =>
            Host.CreateDefaultBuilder()
                .UseDiscoveredModules()
                .ConfigureWebHostDefaults(whb => whb
                    .UseUrls("http://localhost:5000", "http://localhost:50001")
                    .UseStartup<Startup>()
                )
                .ConfigureLogging(lb => lb
                    .SetMinimumLevel(LogLevel.Trace)
                )
                .Build();
    }
}
