using Microsoft.AspNetCore.Mvc;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Reposotories;
using Telhai.CS.DotNet.ItamarErez.TomerHarari.Repositories.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add this diagnostic code
var assembly = Assembly.GetExecutingAssembly();
var controllerTypes = assembly.GetTypes()
    .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
    .ToList();

// Print found controllers
foreach (var controller in controllerTypes)
{
    Console.WriteLine($"Found controller: {controller.Name}");
}

// Add services
builder.Services.AddControllers()
    .AddApplicationPart(Assembly.GetExecutingAssembly());  // Add this line

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Register repositories
builder.Services.AddSingleton<IBugRepository>(provider =>
    BugRepository.Initialize(builder.Configuration));

builder.Services.AddSingleton<IBugCategoryRepository>(provider =>
    BugCategoryRepository.Initialize(builder.Configuration));

var app = builder.Build();

// Configure app
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();