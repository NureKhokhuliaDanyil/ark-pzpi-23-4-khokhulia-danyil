using Microsoft.EntityFrameworkCore;
using Washing.Data;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Services;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;

    public PaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> GetBalanceAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        
        if (user == null)
            throw new InvalidOperationException("User not found");

        return user.Balance;
    }

    public async Task ApplyPromoCodeAsync(int userId, string promoCode)
    {
        var user = await _context.Users.FindAsync(userId);
        
        if (user == null)
            throw new InvalidOperationException("User not found");

        decimal discountAmount = promoCode.ToUpper() switch
        {
            "WASH50" => 50m,
            "CLEAN20" => 20m,
            "SAVE10" => 10m,
            _ => throw new InvalidOperationException("Invalid promo code")
        };

        user.Balance += discountAmount;

        var transaction = new Transaction
        {
            UserId = userId,
            Amount = discountAmount,
            Type = TransactionType.Deposit,
            Timestamp = DateTime.Now
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task ProcessPaymentAsync(int userId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        var user = await _context.Users.FindAsync(userId);
        
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (user.Balance < amount)
            throw new InvalidOperationException("Insufficient balance");

        user.Balance -= amount;

        var transaction = new Transaction
        {
            UserId = userId,
            Amount = amount,
            Type = TransactionType.Payment,
            Timestamp = DateTime.Now
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }
}
