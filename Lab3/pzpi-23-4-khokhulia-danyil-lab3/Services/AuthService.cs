using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Washing.Data;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is blocked");

        var token = GenerateJwtToken(user);

        return new LoginResponseDto(user.Id, user.FullName, user.Email, user.Role, token);
    }

    public async Task<UserResponseDto> RegisterAsync(RegisterUserDto dto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if (existingUser != null)
            throw new InvalidOperationException("Email already registered");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            Balance = 0,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserResponseDto(user.Id, user.FullName, user.Email, user.Role, user.Balance);
    }

    public async Task<ForgotPasswordResponseDto> ForgotPasswordAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
            throw new InvalidOperationException("User not found");

        var resetToken = Guid.NewGuid().ToString();
        user.PasswordResetToken = resetToken;
        
        await _context.SaveChangesAsync();

        return new ForgotPasswordResponseDto("Password reset token generated", resetToken);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];
        var jwtExpiryMinutes = int.Parse(_configuration["Jwt:ExpiryInMinutes"] ?? "60");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
