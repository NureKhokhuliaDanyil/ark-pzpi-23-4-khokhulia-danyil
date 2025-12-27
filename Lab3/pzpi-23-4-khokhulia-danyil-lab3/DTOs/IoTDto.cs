namespace Washing.DTOs;

public record TelemetryDto(string DeviceId, decimal CurrentConsumption, decimal VibrationLevel, decimal Temperature);

public record CommandDto(int MachineId, string Command);

public record HeartbeatDto(string DeviceId);
