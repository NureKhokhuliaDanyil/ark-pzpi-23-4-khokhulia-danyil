namespace Washing.DTOs;

public record CreateSessionDto(int UserId, int MachineId, int ModeId, DateTime StartTime, string Status, decimal ActualPrice);

public record StartSessionDto(int UserId, int MachineId, int ModeId);

public record SessionResponseDto(int Id, int UserId, int MachineId, int ModeId, DateTime StartTime, DateTime? EndTime, string Status, decimal ActualPrice, bool DoorLocked);

public record CancelSessionDto(int SessionId, int UserId);
