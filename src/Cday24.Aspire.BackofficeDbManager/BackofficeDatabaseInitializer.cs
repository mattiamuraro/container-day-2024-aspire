using Cday24.Aspire.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Cday24.Aspire.BackofficeDbManager
{
    internal class BackofficeDatabaseInitializer(IServiceProvider serviceProvider, ILogger<BackofficeDatabaseInitializer> logger)
        : BackgroundService
    {
        public const string ActivitySourceName = "Migrations";

        private readonly ActivitySource _activitySource = new(ActivitySourceName);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<Cday24DbContext>();

            await InitializeDatabaseAsync(dbContext, cancellationToken);
        }

        private async Task InitializeDatabaseAsync(Cday24DbContext dbContext, CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity("Initializing backoffice database", ActivityKind.Client);

            var sw = Stopwatch.StartNew();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);

            await SeedAsync(dbContext, cancellationToken);

            logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
        }

        private async Task SeedAsync(Cday24DbContext dbContext, CancellationToken cancellationToken)
        {
            logger.LogInformation("Seeding database");
        }
    }
}
