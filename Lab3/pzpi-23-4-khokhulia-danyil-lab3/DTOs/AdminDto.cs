namespace Washing.DTOs;

public record SystemStatsDto(int TotalUsers, int ActiveSessions, decimal TotalRevenue, int IdleMachines, int BusyMachines, int MaintenanceMachines);

public record LaundryReportItemDto(string Name, string Address, decimal Revenue);

public record DeviceRegistrationDto(string DeviceId, int MachineId);
