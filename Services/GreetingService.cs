namespace EmployeeManager.Services;

public class GreetingService : IGreetingService
{
    private readonly ILogger<GreetingService> _logger;

    public GreetingService(ILogger<GreetingService> logger)
    {
        _logger = logger;
    }
    
    public void GetGreeting()
    {
       _logger.LogInformation("-----Welcome to the StudentManager ----");
    }
}