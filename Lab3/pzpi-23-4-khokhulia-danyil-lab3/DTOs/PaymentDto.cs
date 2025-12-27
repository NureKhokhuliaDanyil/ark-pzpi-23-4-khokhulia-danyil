namespace Washing.DTOs;

public record PromoCodeDto(string Code);

public record BalanceResponseDto(int UserId, decimal Balance);

public record ProcessPaymentDto(int UserId, decimal Amount);
