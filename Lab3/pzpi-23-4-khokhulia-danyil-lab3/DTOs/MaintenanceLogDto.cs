namespace Washing.DTOs;

public record CreateMaintenanceLogDto(int MachineId, string Description, DateTime StartedAt);

public record UpdateMaintenanceLogDto(int MachineId, string Description, DateTime StartedAt);

public record MaintenanceLogResponseDto(int Id, int MachineId, string Description, DateTime StartedAt, DateTime? ResolvedAt, decimal? Cost);

public record ReportIssueDto(int MachineId, string Description);

public record ResolveIssueDto(int LogId, decimal Cost, string Notes);
