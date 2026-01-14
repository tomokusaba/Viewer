var builder = DistributedApplication.CreateBuilder(args);

// Add SQL Server container
var sqlServer = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent);

var sqlDb = sqlServer.AddDatabase("DigitalSignageDb");

// Add the API Server with Blazor WASM client
var apiServer = builder.AddProject<Projects.DigitalSignage_Server>("api-server")
    .WithReference(sqlDb)
    .WaitFor(sqlDb)
    .WithExternalHttpEndpoints();

builder.Build().Run();
