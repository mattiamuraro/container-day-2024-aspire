using Cday24.Aspire.BackofficeDbManager;
using Cday24.Aspire.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<Cday24DbContext>("backofficedatabase", null,
                                            optionsBuilder => optionsBuilder.UseNpgsql(npgsqlBuilder =>
                                            npgsqlBuilder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

builder.Services.AddOpenTelemetry()
                .WithTracing(tracing => tracing.AddSource(BackofficeDatabaseInitializer.ActivitySourceName));

builder.Services.AddSingleton<BackofficeDatabaseInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<BackofficeDatabaseInitializer>());
builder.Services.AddHealthChecks()
                .AddCheck<BackofficeDatabaseInitializerHealthCheck>("BackofficeDatabaseInitializer", null);

var app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();

