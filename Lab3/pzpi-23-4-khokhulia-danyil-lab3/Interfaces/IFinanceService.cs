using Washing.DTOs;

namespace Washing.Interfaces;

public interface IFinanceService
{
    Task DepositAsync(int userId, decimal amount);
    Task<RevenueStatsDto> GetRevenueStatsAsync();
}
