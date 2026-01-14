using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DigitalSignage.Client;
using DigitalSignage.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register API services
builder.Services.AddScoped<ContentService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<TemplateService>();

await builder.Build().RunAsync();
