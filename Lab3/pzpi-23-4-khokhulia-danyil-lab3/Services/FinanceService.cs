using Microsoft.EntityFrameworkCore;
using Washing.Data;
using Washing.DTOs;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Services;

public class FinanceService : IFinanceService
{
    private readonly ApplicationDbContext _context;

    public FinanceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DepositAsync(int userId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        var user = await _context.Users.FindAsync(userId);
        
        if (user == null)
            throw new InvalidOperationException("User not found");

        user.Balance += amount;

        var transaction = new Transaction
        {
            UserId = userId,
            Amount = amount,
            Type = TransactionType.Deposit,
            Timestamp = DateTime.Now
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<RevenueStatsDto> GetRevenueStatsAsync()
    {
        var allPayments = await _context.Transactions
            .Where(t => t.Type == TransactionType.Payment)
            .ToListAsync();

        var totalRevenue = allPayments.Sum(t => t.Amount);

        var today = DateTime.Today;
        var todayRevenue = allPayments
            .Where(t => t.Timestamp.Date == today)
            .Sum(t => t.Amount);

        var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var monthRevenue = allPayments
            .Where(t => t.Timestamp >= firstDayOfMonth)
            .Sum(t => t.Amount);

        var revenueByLaundry = await _context.WashingSessions
            .Include(s => s.Machine)
            .GroupBy(s => s.Machine.LaundryId)
            .Select(g => new { LaundryId = g.Key, Revenue = g.Sum(s => s.ActualPrice) })
            .ToDictionaryAsync(x => x.LaundryId, x => x.Revenue);

        return new RevenueStatsDto(totalRevenue, todayRevenue, monthRevenue, revenueByLaundry);
    }
}
