using ZooAssignment.Server.Models;
using ZooAssignment.Server.Services;
using ZooAssignment.Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣  CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev", policy =>
    {
        policy
            .WithOrigins("https://localhost:56116")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.Configure<DataFilesOptions>(
    builder.Configuration.GetSection("DataFiles"));

builder.Services.AddTransient<IPriceProvider,FilePriceProvider>();
builder.Services.AddTransient<IAnimalInfoProvider, FileAnimalInfoProvider>();
builder.Services.AddTransient<IZooProvider, FileZooProvider>();
builder.Services.AddTransient<ICostCalculator, DailyCostCalculator>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowViteDev");
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/dailycost", ( ICostCalculator calc) =>
{
    // Call your business logic
    var dailyCost = calc.CalculateDailyCost();
    // Return as JSON
    return Results.Ok(new { dailyCost });
})
.WithName("CalculateDailyCost")
.Produces<double>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status400BadRequest);

// 5️⃣  SPA fallback
app.MapFallbackToFile("index.html");

app.Run();


