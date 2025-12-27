using Washing.DTOs;

namespace Washing.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
    Task<UserResponseDto> RegisterAsync(RegisterUserDto dto);
    Task<ForgotPasswordResponseDto> ForgotPasswordAsync(string email);
}
