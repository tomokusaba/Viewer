using DigitalSignage.Server.Data;
using DigitalSignage.Server.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (health checks, telemetry, etc.)
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddOpenApi();

// Add CORS for Blazor WASM client
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add DbContext with Aspire SQL Server integration
builder.AddSqlServerDbContext<ApplicationDbContext>("DigitalSignageDb");

var app = builder.Build();

// Map Aspire default endpoints (health, alive)
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

// Serve Blazor client static files
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

// Map API endpoints
app.MapContentEndpoints();
app.MapTagEndpoints();
app.MapTemplateEndpoints();

// Fallback to index.html for Blazor routing
app.MapFallbackToFile("index.html");

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
