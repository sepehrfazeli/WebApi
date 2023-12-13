using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Build the connection string
var connectionString = $"server={Env.GetString("DB_HOST")};port={Env.GetString("DB_PORT")};database={Env.GetString("DB_NAME")};user={Env.GetString("DB_USER")};password={Env.GetString("DB_PASS")}";

// Add services to the container.
builder.Services.AddDbContext<StudentManagementContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 2, 0))));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}