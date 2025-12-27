using Microsoft.EntityFrameworkCore;
using Washing.Data;
using Washing.DTOs;
using Washing.Interfaces;

namespace Washing.Services;

public class PricingService : IPricingService
{
    private readonly ApplicationDbContext _context;

    public PricingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> CalculatePriceAsync(int machineId, int modeId)
    {
        var mode = await _context.WashModes.FindAsync(modeId);
        
        if (mode == null)
            throw new InvalidOperationException("Wash mode not found");

        var basePrice = mode.Price;
        var timeMultiplier = await GetTimeMultiplierAsync();
        var loadMultiplier = await GetLoadMultiplierAsync();

        var finalPrice = basePrice * timeMultiplier * loadMultiplier;

        return Math.Round(finalPrice, 2);
    }

    public async Task<PricingDetailDto> CalculateFinalPriceAsync(int laundryId, int modeId, int userId)
    {
        var mode = await _context.WashModes.FindAsync(modeId);
        if (mode == null)
            throw new InvalidOperationException("Wash mode not found");

        var basePrice = mode.Price;
        var modifiers = new List<string>();
        var finalPrice = basePrice;

        var timeMultiplier = await GetTimeMultiplierAsync();
        if (timeMultiplier != 1.0m)
        {
            finalPrice *= timeMultiplier;
            modifiers.Add($"Time discount: {(timeMultiplier < 1 ? (1 - timeMultiplier) * 100 : (timeMultiplier - 1) * 100):F0}%");
        }

        var loadMultiplier = await GetLoadMultiplierAsync(laundryId);
        if (loadMultiplier != 1.0m)
        {
            finalPrice *= loadMultiplier;
            modifiers.Add($"Peak load surcharge: {(loadMultiplier - 1) * 100:F0}%");
        }

        var completedSessions = await _context.WashingSessions
            .CountAsync(s => s.UserId == userId && s.Status == "Completed");

        if (completedSessions > 10)
        {
            finalPrice *= 0.95m;
            modifiers.Add("Loyalty discount: 5%");
        }

        if (modifiers.Count == 0)
        {
            modifiers.Add("Standard pricing");
        }

        return new PricingDetailDto(basePrice, Math.Round(finalPrice, 2), modifiers);
    }

    private async Task<decimal> GetTimeMultiplierAsync()
    {
        var currentTime = DateTime.Now.TimeOfDay;

        var activeCondition = await _context.TimePricingConditions
            .Where(c => c.IsActive && c.TimeStart <= currentTime && c.TimeEnd >= currentTime)
            .FirstOrDefaultAsync();

        return activeCondition?.Multiplier ?? 1.0m;
    }

    private async Task<decimal> GetLoadMultiplierAsync()
    {
        var totalMachines = await _context.WashingMachines.CountAsync();
        
        if (totalMachines == 0)
            return 1.0m;

        var busyMachines = await _context.WashingMachines
            .CountAsync(m => m.Status == Entities.MachineStatus.Busy);

        var loadFactor = (decimal)busyMachines / totalMachines;

        var activeCondition = await _context.LoadPricingConditions
            .Where(c => c.IsActive && c.LoadThreshold <= loadFactor)
            .OrderByDescending(c => c.LoadThreshold)
            .FirstOrDefaultAsync();

        return activeCondition?.Multiplier ?? 1.0m;
    }

    private async Task<decimal> GetLoadMultiplierAsync(int laundryId)
    {
        var totalMachines = await _context.WashingMachines
            .CountAsync(m => m.LaundryId == laundryId);
        
        if (totalMachines == 0)
            return 1.0m;

        var busyMachines = await _context.WashingMachines
            .CountAsync(m => m.LaundryId == laundryId && m.Status == Entities.MachineStatus.Busy);

        var loadFactor = (decimal)busyMachines / totalMachines;

        var activeCondition = await _context.LoadPricingConditions
            .Where(c => c.IsActive && c.LoadThreshold <= loadFactor)
            .OrderByDescending(c => c.LoadThreshold)
            .FirstOrDefaultAsync();

        return activeCondition?.Multiplier ?? 1.0m;
    }
}
