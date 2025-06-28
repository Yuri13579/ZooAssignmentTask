using ZooAssignment.Server.Services;
using ZooAssignment.Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣  Register CORS and specify your React/Vite origin:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteDev", policy =>
    {
        policy
            .WithOrigins("https://localhost:56116")   // ← your React app’s URL
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddTransient<ICostCalculator, DailyCostCalculator>();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
// 3️⃣  Enable the CORS policy _before_ your endpoints
app.UseCors("AllowViteDev");

app.UseDefaultFiles();
app.UseStaticFiles();

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

app.MapFallbackToFile("/index.html");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
