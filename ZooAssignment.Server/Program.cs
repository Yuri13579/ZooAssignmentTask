var builder = WebApplication.CreateBuilder(args);

// Register CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("vite",
        p => p.WithOrigins("https://localhost:56116/") // Vite dev origin
            .AllowAnyHeader()
            .AllowAnyMethod());
});


// Add services to the container.

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
// Enable CORS before the endpoints
app.UseHttpsRedirection();
app.UseCors("vite");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapFallbackToFile("/index.html");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
