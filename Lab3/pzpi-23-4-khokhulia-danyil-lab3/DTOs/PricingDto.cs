namespace Washing.DTOs;

public record PricingDetailDto(
    decimal BasePrice,
    decimal FinalPrice,
    List<string> AppliedModifiers
);

public record BusyHourDto(
    int Hour,
    int SessionCount,
    decimal TotalRevenue,
    string LoadLevel
);

public record CreateTimePricingDto(TimeSpan TimeStart, TimeSpan TimeEnd, decimal Multiplier);

public record CreateLoadPricingDto(decimal LoadThreshold, decimal Multiplier);
