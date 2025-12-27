namespace Washing.Entities;

public enum UserRole
{
    Client = 0,
    Admin = 1,
    Technician = 2
}

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public decimal Balance { get; set; }
    public string? PasswordResetToken { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Laundry> OwnedLaundries { get; set; } = new List<Laundry>();
    public ICollection<WashingSession> WashingSessions { get; set; } = new List<WashingSession>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
