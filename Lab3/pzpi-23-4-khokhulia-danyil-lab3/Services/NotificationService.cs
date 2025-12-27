using Microsoft.EntityFrameworkCore;
using Washing.Data;
using Washing.Entities;
using Washing.Interfaces;

namespace Washing.Services;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateNotificationAsync(int userId, string title, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            IsRead = false
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }
}
