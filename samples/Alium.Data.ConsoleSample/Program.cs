namespace Alium.Data.ConsoleSample
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Alium.Administration;
    using Alium.Modules;
    using System.Threading;

    class Program
    {
        public static async Task Main(string[] args)
        {
            await BuildHost(args).RunConsoleAsync();
        }

        public static IHostBuilder BuildHost(string[] args) =>
            Host.CreateDefaultBuilder()
                .UseDiscoveredModules()
                .ConfigureServices(services =>
                {
                    services.AddEntityFrameworkSqlServer();
                    services.AddDbContext<AdministrationUserDbContext>(options =>
                    {
                        options.UseSqlServer("Server=(localdb)\\MSSqlLocalDB;Database=alium-feature-entities;Integrated Security=SSPI");
                    });
                    services.AddScoped<IAdministrationUserReader, AdministrationUserReader>();
                    services.AddSingleton<IHostedService, TestHostedService>();
                })
                .ConfigureLogging(lb => lb
                    .SetMinimumLevel(LogLevel.Trace)
                );
    }

    public class TestHostedService : IHostedService
    {
        private readonly IServiceScope _scope;

        public TestHostedService(IServiceProvider services)
        {
            _scope = services.CreateScope();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var service = _scope.ServiceProvider.GetRequiredService<IAdministrationUserReader>();
            var user = await service.GetByIdAsync(new AdministrationUserId(0));


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _scope.Dispose();

            return Task.CompletedTask;
        }
    }

    public class AdministrationUserDbContext : DbContext
    {
        public DbSet<AdministrationUser> Users { get; set; } = default!;

        public AdministrationUserDbContext(DbContextOptions<AdministrationUserDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AdministrationUser>(etp =>
            {
                etp.Property(u => u.Id).HasConversion(new AdministrationUserIdValueConverter()).IsRequired();
            });
        }
    }

    public class AdministrationUserReader : DbContextReader<AdministrationUserDbContext, AdministrationUser, AdministrationUserId>, IAdministrationUserReader
    {
        public AdministrationUserReader(AdministrationUserDbContext context)
            : base(context)
        {

        }
    }

    public interface IAdministrationUserReader : IReader<AdministrationUser, AdministrationUserId>
    {

    }

    public class AdministrationUserIdValueConverter : ValueConverter<AdministrationUserId, int>
    {
        public AdministrationUserIdValueConverter()
            : base(id => id.Value, id => new AdministrationUserId(id))
        {

        }
    }
}
