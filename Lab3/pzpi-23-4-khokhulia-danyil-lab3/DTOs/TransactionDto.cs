using Washing.Entities;

namespace Washing.DTOs;

public record TransactionResponseDto(int Id, int UserId, decimal Amount, TransactionType Type, DateTime Timestamp);
