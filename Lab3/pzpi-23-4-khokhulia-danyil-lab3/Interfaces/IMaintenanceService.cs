namespace Washing.Interfaces;

public interface IMaintenanceService
{
    Task<int> ReportIssueAsync(int machineId, string description);
    Task ResolveIssueAsync(int logId, decimal cost, string notes);
}
