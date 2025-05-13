using EmployeeManager.Models;

namespace EmployeeManager.Middlewares;

public class DatabaseConnectionMiddleware 
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DatabaseConnectionMiddleware> _logger;
 

    public DatabaseConnectionMiddleware( RequestDelegate next,
        ILogger<DatabaseConnectionMiddleware> logger)
    {
        _next = next;
        _logger = logger;

    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
    {
        try
        {
            // Check database connection
            var canConnect = await dbContext.Database.CanConnectAsync();
                
            if (canConnect)
            {
                _logger.LogInformation("------ Database connection successful!!! ------");
                // Add header to response to indicate DB status
                context.Response.Headers.Add("X-Database-Status", "Connected");
            }
            else
            {
                _logger.LogWarning("------ Database connection failed ------");
                context.Response.Headers.Add("X-Database-Status", "Disconnected");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connection error");
            context.Response.Headers.Add("X-Database-Status", $"Error: {ex.Message}");
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}