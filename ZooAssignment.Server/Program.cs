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

// 3️⃣  Existing weather endpoint
app.MapGet("/weatherforecast", () =>
{
    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    return Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
    .ToArray();
});

// 4️⃣  New calculate endpoint
//    - Expects a JSON body matching CalculateRequest
//    - Injects ICostCalculator from DI
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


// ——— DTOs & Records ———

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

