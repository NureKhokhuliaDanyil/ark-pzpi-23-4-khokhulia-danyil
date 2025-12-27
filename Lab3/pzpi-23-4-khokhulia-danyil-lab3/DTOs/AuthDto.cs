using Washing.Entities;

namespace Washing.DTOs;

public record LoginDto(string Email, string Password);

public record LoginResponseDto(int UserId, string FullName, string Email, UserRole Role, string Token);

public record ForgotPasswordResponseDto(string Message, string ResetToken);
