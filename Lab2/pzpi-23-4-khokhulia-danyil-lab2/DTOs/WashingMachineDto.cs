using Washing.Entities;

namespace Washing.DTOs;

public record CreateMachineDto(int LaundryId, string SerialNumber, string Model, MachineStatus Status);

public record UpdateMachineDto(int LaundryId, string SerialNumber, string Model, MachineStatus Status);

public record MachineResponseDto(int Id, int LaundryId, string SerialNumber, string Model, MachineStatus Status);
