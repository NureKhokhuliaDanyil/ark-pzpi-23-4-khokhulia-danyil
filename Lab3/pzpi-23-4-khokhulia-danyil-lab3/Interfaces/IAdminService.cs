using Washing.DTOs;

namespace Washing.Interfaces;

public interface IAdminService
{
    Task<SystemStatsDto> GetStatsAsync();
    Task<byte[]> ExportLaundriesReportAsync();
    Task<int> CreateTimePricingConditionAsync(CreateTimePricingDto dto);
    Task<int> CreateLoadPricingConditionAsync(CreateLoadPricingDto dto);
    Task BlockUserAsync(int userId);
    Task<List<BusyHourDto>> GetBusyHoursAnalyticsAsync(int laundryId);
}
