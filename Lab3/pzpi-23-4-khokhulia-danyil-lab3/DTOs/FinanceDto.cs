namespace Washing.DTOs;

public record DepositDto(int UserId, decimal Amount);

public record RevenueStatsDto(decimal TotalRevenue, decimal TodayRevenue, decimal MonthRevenue, Dictionary<int, decimal> RevenueByLaundry);
