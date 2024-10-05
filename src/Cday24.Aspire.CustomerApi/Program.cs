using Cday24.Aspire.CustomerApi.Extensions;
using Cday24.Aspire.CustomerApi.Services;
using Cday24.Aspire.Models.Options;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("messaging");

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq(
        (context, cfg) =>
        {
            var host = builder.Configuration.GetConnectionString("messaging");

            cfg.Host(host);
            cfg.ConfigureEndpoints(context);
        }
    );
});

builder.Services.AddProblemDetails();

builder.Services.AddScoped<TicketService>();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("rabbitMq"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseExceptionHandler();
app.MapApplicationEndpoints();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();