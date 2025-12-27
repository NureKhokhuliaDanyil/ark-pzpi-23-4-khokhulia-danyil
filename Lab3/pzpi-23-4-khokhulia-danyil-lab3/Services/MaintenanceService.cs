using Microsoft.EntityFrameworkCore;
using Washing.Data;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Services;

public class MaintenanceService : IMaintenanceService
{
    private readonly ApplicationDbContext _context;

    public MaintenanceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> ReportIssueAsync(int machineId, string description)
    {
        var machine = await _context.WashingMachines.FindAsync(machineId);
        
        if (machine == null)
            throw new InvalidOperationException("Machine not found");

        machine.Status = MachineStatus.Maintenance;

        var log = new MaintenanceLog
        {
            MachineId = machineId,
            Description = description,
            StartedAt = DateTime.Now
        };

        _context.MaintenanceLogs.Add(log);
        await _context.SaveChangesAsync();

        return log.Id;
    }

    public async Task ResolveIssueAsync(int logId, decimal cost, string notes)
    {
        var log = await _context.MaintenanceLogs
            .Include(l => l.Machine)
            .FirstOrDefaultAsync(l => l.Id == logId);

        if (log == null)
            throw new InvalidOperationException("Maintenance log not found");

        log.ResolvedAt = DateTime.Now;
        log.Cost = cost;
        log.Description += $"\n\nResolution: {notes}";

        log.Machine.Status = MachineStatus.Idle;

        await _context.SaveChangesAsync();
    }
}
