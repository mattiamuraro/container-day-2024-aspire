using Cday24.Aspire.BackofficeApi.Consumers.Definitions;
using Cday24.Aspire.BackofficeApi.Consumers.Handlers;
using Cday24.Aspire.BackofficeApi.Extensions;
using Cday24.Aspire.BackofficeApi.HttpClients;
using Cday24.Aspire.BackofficeApi.Services;
using Cday24.Aspire.Data;
using Cday24.Aspire.Models.Options;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddAzureOpenAIClient("azureopenaigpt4o");
builder.AddNpgsqlDbContext<Cday24DbContext>("backofficedatabase");

builder.Services.AddProblemDetails();

builder.Services.AddScoped<OpenAiService>();
builder.Services.AddScoped<TicketService>();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("rabbitMq"));
builder.Services.Configure<AzureOpenAiSettings>(builder.Configuration.GetSection("azureopenai"));

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<TicketRequestMessageConsumer, TicketRequestMessageDefinition>();

    x.UsingRabbitMq(
        (context, cfg) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var host = configuration.GetConnectionString("messaging");

            cfg.Host(host);
            cfg.ConfigureEndpoints(context);
        }
    );
});

builder.Services.AddHttpClient<WeightApiClient>(client =>
{
    client.BaseAddress = new("https+http://weightApi");
});

var app = builder.Build();
app.UseExceptionHandler();
app.MapApplicationEndpoints();
app.MapDefaultEndpoints();

app.Run();
