namespace Washing.DTOs;

public record CreateSessionDto(int UserId, int MachineId, int ModeId, DateTime StartTime, string Status, decimal ActualPrice);

public record SessionResponseDto(int Id, int UserId, int MachineId, int ModeId, DateTime StartTime, DateTime? EndTime, string Status, decimal ActualPrice);
