namespace Washing.Interfaces;

public interface INotificationService
{
    Task CreateNotificationAsync(int userId, string title, string message);
}
