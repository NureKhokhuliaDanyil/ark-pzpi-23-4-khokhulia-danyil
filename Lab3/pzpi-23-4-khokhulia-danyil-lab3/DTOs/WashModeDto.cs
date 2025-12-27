namespace Washing.DTOs;

public record CreateWashModeDto(int LaundryId, string Name, decimal Price, int DurationMinutes);

public record UpdateWashModeDto(int LaundryId, string Name, decimal Price, int DurationMinutes);

public record WashModeResponseDto(int Id, int LaundryId, string Name, decimal Price, int DurationMinutes);
