using Washing.DTOs;

namespace Washing.Interfaces;

public interface IPricingService
{
    Task<decimal> CalculatePriceAsync(int machineId, int modeId);
    Task<PricingDetailDto> CalculateFinalPriceAsync(int laundryId, int modeId, int userId);
}
