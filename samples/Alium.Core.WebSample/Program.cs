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

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseDiscoveredModules()
                .UseUrls("http://localhost:5000", "http://localhost:5001")
                .UseStartup<Startup>()
                .ConfigureLogging(lb =>
                {
                    lb.SetMinimumLevel(LogLevel.Trace);
                })
                .Build();
    }
}
