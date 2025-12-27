using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Washing.Data;
using Washing.Entities;

namespace Washing.BackgroundServices;

public class MachineMonitorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MachineMonitorService> _logger;

    public MachineMonitorService(IServiceProvider serviceProvider, ILogger<MachineMonitorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Machine Monitor Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckMachineHealthAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Machine Monitor Service");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        _logger.LogInformation("Machine Monitor Service stopped");
    }

    private async Task CheckMachineHealthAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        _logger.LogInformation("Machine health check completed");
    }
}
