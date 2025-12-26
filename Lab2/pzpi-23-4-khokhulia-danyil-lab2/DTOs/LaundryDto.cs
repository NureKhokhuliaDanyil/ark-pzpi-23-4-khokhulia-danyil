using Washing.Entities;

namespace Washing.DTOs;

public record CreateLaundryDto(int OwnerId, string Name, string Address, string WorkingHours);

public record UpdateLaundryDto(int OwnerId, string Name, string Address, string WorkingHours);

public record LaundryResponseDto(int Id, int OwnerId, string Name, string Address, string WorkingHours);
