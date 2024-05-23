using Chat.Components;
using Chat.Models;
using Chat.Services;
using RestEase.HttpClientFactory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRestEaseClient<IEshopApi>(builder.Configuration["API_HOST"]);
builder.Services.AddKeyedTransient<IAssistantService, WeatherAssistantService>("weather");
builder.Services.AddKeyedTransient<IAssistantService, EshopAssistantService>("eshop");
builder.Services.AddKeyedTransient<IAssistantService, SemanticKernelAssistantService>("kernel");
builder.Services.AddKeyedTransient<IAssistantService, AmazonAssistantService>("amazon");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
