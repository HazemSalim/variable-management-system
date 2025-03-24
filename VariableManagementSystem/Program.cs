using Microsoft.EntityFrameworkCore;
using Serilog;
using VariableManagementSystem.Data;
using VariableManagementSystem.Repositories;
using VariableManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Logs to the console
    .WriteTo.File("logs/variables.log", rollingInterval: RollingInterval.Day) // Logs to a file, rolling daily
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging across the application

// Add database context (SQLite in this case)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=variables.db"));

// Register the repositories and services for Dependency Injection
builder.Services.AddScoped<IVariableRepository, VariableRepository>();
builder.Services.AddScoped<IVariableService, VariableService>();

// Add SignalR service
builder.Services.AddSignalR();

// Add controllers for API endpoints
builder.Services.AddControllers();

// Configure Swagger (for API documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply any pending migrations and ensure the database is up to date
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Ensure database schema is up-to-date
}

// Enable Swagger UI for development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enables Swagger
    app.UseSwaggerUI(); // Swagger UI for interactive API documentation
}

// Use Serilog request logging (logs HTTP request details)
app.UseSerilogRequestLogging();

// Enable HTTPS redirection (for production security)
app.UseHttpsRedirection();

// Enable authorization (for authenticated endpoints)
app.UseAuthorization();

// Map the controller routes to their respective endpoints
app.MapControllers();

// Serve static files from the wwwroot folder (e.g., HTML, CSS, JS)
app.UseStaticFiles(); // Serves static files like .html, .css, .js, etc.

// Map the SignalR Hub to handle real-time communication
app.MapHub<VariableHub>("/variableHub");

// Start the application
app.Run();
