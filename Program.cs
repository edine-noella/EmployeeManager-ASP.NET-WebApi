using EmployeeManager.Middlewares;
using EmployeeManager.Models;
using EmployeeManager.Services;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // This enables controller support

// Add DbContext with PostgresSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//all possible lifetimes
// builder.Services.AddTransient<IGreetingService, GreetingService>();
// builder.Services.AddScoped<IGreetingService, GreetingService>();
builder.Services.AddSingleton<IGreetingService, GreetingService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<DatabaseConnectionMiddleware>();

app.UseAuthorization();

// Map your controllers
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var greetingService = scope.ServiceProvider.GetRequiredService<IGreetingService>();
    greetingService.GetGreeting();
}

app.Run();