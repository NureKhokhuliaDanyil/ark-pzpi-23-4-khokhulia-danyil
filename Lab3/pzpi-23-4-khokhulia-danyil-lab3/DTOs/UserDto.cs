using Washing.Entities;

namespace Washing.DTOs;

public record RegisterUserDto(string FullName, string Email, string Password, UserRole Role);

public record UpdateUserDto(string FullName, string Email, string? Password, UserRole Role, decimal Balance);

public record UserResponseDto(int Id, string FullName, string Email, UserRole Role, decimal Balance);
