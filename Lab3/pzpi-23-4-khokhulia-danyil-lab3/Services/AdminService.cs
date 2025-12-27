using Microsoft.EntityFrameworkCore;
using System.Text;
using Washing.Data;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;

    public AdminService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SystemStatsDto> GetStatsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var activeSessions = await _context.WashingSessions.CountAsync(s => s.Status == "Active");
        
        var totalRevenue = await _context.Transactions
            .Where(t => t.Type == TransactionType.Payment)
            .SumAsync(t => t.Amount);

        var idleMachines = await _context.WashingMachines.CountAsync(m => m.Status == MachineStatus.Idle);
        var busyMachines = await _context.WashingMachines.CountAsync(m => m.Status == MachineStatus.Busy);
        var maintenanceMachines = await _context.WashingMachines.CountAsync(m => m.Status == MachineStatus.Maintenance);

        return new SystemStatsDto(totalUsers, activeSessions, totalRevenue, idleMachines, busyMachines, maintenanceMachines);
    }

    public async Task<byte[]> ExportLaundriesReportAsync()
    {
        var laundries = await _context.Laundries
            .Include(l => l.WashingMachines)
            .ThenInclude(m => m.WashingSessions)
            .ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Name,Address,Revenue");

        foreach (var laundry in laundries)
        {
            var revenue = laundry.WashingMachines
                .SelectMany(m => m.WashingSessions)
                .Sum(s => s.ActualPrice);

            csv.AppendLine($"\"{laundry.Name}\",\"{laundry.Address}\",{revenue}");
        }

        return Encoding.UTF8.GetBytes(csv.ToString());
    }

    public async Task<int> CreateTimePricingConditionAsync(CreateTimePricingDto dto)
    {
        var condition = new TimePricingCondition
        {
            TimeStart = dto.TimeStart,
            TimeEnd = dto.TimeEnd,
            Multiplier = dto.Multiplier,
            IsActive = true
        };

        _context.TimePricingConditions.Add(condition);
        await _context.SaveChangesAsync();

        return condition.Id;
    }

    public async Task<int> CreateLoadPricingConditionAsync(CreateLoadPricingDto dto)
    {
        var condition = new LoadPricingCondition
        {
            LoadThreshold = dto.LoadThreshold,
            Multiplier = dto.Multiplier,
            IsActive = true
        };

        _context.LoadPricingConditions.Add(condition);
        await _context.SaveChangesAsync();

        return condition.Id;
    }

    public async Task BlockUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        user.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public async Task<List<BusyHourDto>> GetBusyHoursAnalyticsAsync(int laundryId)
    {
        var sessions = await _context.WashingSessions
            .Include(s => s.Machine)
            .Where(s => s.Machine.LaundryId == laundryId && s.Status == "Completed")
            .ToListAsync();

        var hourlyStats = sessions
            .GroupBy(s => s.StartTime.Hour)
            .Select(g => new
            {
                Hour = g.Key,
                SessionCount = g.Count(),
                TotalRevenue = g.Sum(s => s.ActualPrice),
                LoadLevel = g.Count() > 10 ? "High" : g.Count() > 5 ? "Medium" : "Low"
            })
            .OrderByDescending(x => x.SessionCount)
            .Select(x => new BusyHourDto(x.Hour, x.SessionCount, x.TotalRevenue, x.LoadLevel))
            .ToList();

        return hourlyStats;
    }
}
