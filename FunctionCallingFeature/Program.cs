using FunctionCallingFeature.Data;
using FunctionCallingFeature.Functions.Weather;
using FunctionCallingFeature.Services;
using FunctionCallingFeature.Services.EShop;
using FunctionCallingFeature.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddKeyedTransient<IAssistantService, WeatherAssistantService>("weatherAssistant");
builder.Services.AddKeyedTransient<IAssistantService, EShopAssistantService>("eshopAssistant");
builder.Services.AddKeyedTransient<IAssistantService, AmazonAssistantService>("amazonAssistant");
builder.Services.AddDbContext<EShopDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Database")!));
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IPurchaseService, PurchaseService>();

// Semantic Kernel
builder.Services.AddOpenAIChatCompletion("gpt-3.5-turbo", builder.Configuration["API_KEY"]);
builder.Services.AddTransient<Kernel>(sp =>
{
    Kernel k = new(sp);
    k.ImportPluginFromType<CartService>();
    k.ImportPluginFromType<OrderService>();
    k.ImportPluginFromType<CatalogService>();
    k.ImportPluginFromType<PurchaseService>();
    k.ImportPluginFromType<GetCapitalFunction>();
    k.ImportPluginFromType<GetWeatherFunction>();
    return k;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
