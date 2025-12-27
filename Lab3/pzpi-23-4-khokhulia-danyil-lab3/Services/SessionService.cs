using Microsoft.EntityFrameworkCore;
using Washing.Data;
using Washing.DTOs;
using Washing.Entities;
using Washing.Extensions;
using Washing.Interfaces;

namespace Washing.Services;

public class SessionService : ISessionService
{
    private readonly ApplicationDbContext _context;
    private readonly IPricingService _pricingService;
    private readonly INotificationService _notificationService;

    public SessionService(ApplicationDbContext context, IPricingService pricingService, INotificationService notificationService)
    {
        _context = context;
        _pricingService = pricingService;
        _notificationService = notificationService;
    }

    public async Task<SessionResponseDto> StartSessionAsync(StartSessionDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var machine = await _context.WashingMachines.FindAsync(dto.MachineId);
            if (machine == null)
                throw new InvalidOperationException("Machine not found");

            if (!machine.IsAvailable())
                throw new InvalidOperationException("Machine is not available");

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var mode = await _context.WashModes.FindAsync(dto.ModeId);
            if (mode == null)
                throw new InvalidOperationException("Wash mode not found");

            var calculatedPrice = await _pricingService.CalculatePriceAsync(dto.MachineId, dto.ModeId);

            if (!user.HasSufficientBalance(calculatedPrice))
                throw new InvalidOperationException("Insufficient balance");

            user.Balance -= calculatedPrice;

            var paymentTransaction = new Transaction
            {
                UserId = dto.UserId,
                Amount = calculatedPrice,
                Type = TransactionType.Payment,
                Timestamp = DateTime.Now
            };
            _context.Transactions.Add(paymentTransaction);

            machine.Status = MachineStatus.Busy;

            var session = new WashingSession
            {
                UserId = dto.UserId,
                MachineId = dto.MachineId,
                ModeId = dto.ModeId,
                StartTime = DateTime.Now,
                Status = "Active",
                ActualPrice = calculatedPrice,
                DoorLocked = true
            };

            _context.WashingSessions.Add(session);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new SessionResponseDto(session.Id, session.UserId, session.MachineId, session.ModeId, session.StartTime, session.EndTime, session.Status, session.ActualPrice, session.DoorLocked);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task CompleteSessionAsync(int sessionId)
    {
        var session = await _context.WashingSessions
            .Include(s => s.Machine)
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session == null)
            throw new InvalidOperationException("Session not found");

        session.Status = "Completed";
        session.EndTime = DateTime.Now;
        session.DoorLocked = false;

        session.Machine.Status = MachineStatus.Idle;

        var cashbackAmount = Math.Round(session.ActualPrice * 0.05m, 2);
        session.User.Balance += cashbackAmount;

        var cashbackTransaction = new Transaction
        {
            UserId = session.UserId,
            Amount = cashbackAmount,
            Type = TransactionType.Deposit,
            Timestamp = DateTime.Now
        };
        _context.Transactions.Add(cashbackTransaction);

        await _notificationService.CreateNotificationAsync(
            session.UserId,
            "Laundry Complete",
            $"Your laundry is done! You earned {cashbackAmount:C} cashback."
        );

        await _context.SaveChangesAsync();
    }

    public async Task CancelSessionAsync(int sessionId, int userId)
    {
        var session = await _context.WashingSessions
            .Include(s => s.Machine)
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session == null)
            throw new InvalidOperationException("Session not found");

        if (session.UserId != userId)
            throw new UnauthorizedAccessException("You can only cancel your own sessions");

        if (session.Status == "Completed" || session.Status == "Cancelled")
            throw new InvalidOperationException("Session already completed or cancelled");

        var hoursUntilStart = (session.StartTime - DateTime.Now).TotalHours;
        decimal refundAmount;
        string refundReason;

        if (session.Status == "Active")
        {
            throw new Exceptions.BusinessRuleException("Cannot cancel an active session");
        }
        else if (hoursUntilStart > 1)
        {
            refundAmount = session.ActualPrice;
            refundReason = "Full refund (>1 hour notice)";
        }
        else if (hoursUntilStart > 0)
        {
            refundAmount = session.ActualPrice * 0.5m;
            refundReason = "50% refund (<1 hour notice)";
        }
        else
        {
            throw new Exceptions.BusinessRuleException("Too late to cancel");
        }

        session.User.Balance += refundAmount;

        var refundTransaction = new Transaction
        {
            UserId = userId,
            Amount = refundAmount,
            Type = TransactionType.Deposit,
            Timestamp = DateTime.Now
        };
        _context.Transactions.Add(refundTransaction);

        session.Status = "Cancelled";
        session.EndTime = DateTime.Now;
        session.DoorLocked = false;

        session.Machine.Status = MachineStatus.Idle;

        await _notificationService.CreateNotificationAsync(
            userId,
            "Session Cancelled",
            $"{refundReason}: {refundAmount:C}"
        );

        await _context.SaveChangesAsync();
    }
}
