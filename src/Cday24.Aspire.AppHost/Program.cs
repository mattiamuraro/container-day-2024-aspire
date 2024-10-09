var builder = DistributedApplication.CreateBuilder(args);

var queueName = "customer-ticket-queue";
var azureOpenAiDeploymentName = "customer-ticket-analyzer";

var username = builder.AddParameter("rabbitMqUsername", secret: true);
var password = builder.AddParameter("rabbitMqPassword", secret: true);
var messaging = builder.AddRabbitMQ("messaging", username, password)
                       .WithManagementPlugin();

var backofficePostgreSQL = builder.AddPostgres("backofficepostgresql")
                      .WithDataVolume("backofficepostgresdatavolume")
                      .WithPgAdmin();

var backofficeDatabase = backofficePostgreSQL.AddDatabase("backofficedatabase");

var azureOpenAi = builder.AddAzureOpenAI("azureopenaigpt4o")
                         .AddDeployment(new AzureOpenAIDeployment(azureOpenAiDeploymentName, "gpt-4o", "2024-05-13", "GlobalStandard", 10));

var backofficedbmanager = builder.AddProject<Projects.Cday24_Aspire_BackofficeDbManager>("backofficedbmanager")
                                 .WithReference(backofficeDatabase);

var weightApi = builder.AddPythonProject("weightapi", "../Cday24.Aspire.WeightApi", "app.py")
                              .WithEndpoint(scheme: "http", env: "PORT");

var customerApi = builder.AddProject<Projects.Cday24_Aspire_CustomerApi>("customerapi")
                         .WithEnvironment("rabbitMq__queueName", queueName)
                         .WithReference(messaging);

var customerApp = builder.AddProject<Projects.Cday24_Aspire_CustomerApp>("customerapp")
                         .WithExternalHttpEndpoints()
                         .WithReference(customerApi);

var backofficeApi = builder.AddProject<Projects.Cday24_Aspire_BackofficeApi>("backofficeapi")
                           .WithEnvironment("rabbitMq__queueName", queueName)
                           .WithEnvironment("azureOpenAi__deploymentName", azureOpenAiDeploymentName)
                           .WithReference(messaging)
                           .WithReference(weightApi)
                           .WithReference(azureOpenAi)
                           .WithReference(backofficeDatabase);

var backofficeApp = builder.AddNpmApp("backofficeapp", "../Cday24.Aspire.BackofficeApp")
                           .WithHttpEndpoint(env: "PORT")
                           .WithExternalHttpEndpoints()
                           .PublishAsDockerFile()
                           .WithReference(backofficeApi)
;

builder.Build().Run();
