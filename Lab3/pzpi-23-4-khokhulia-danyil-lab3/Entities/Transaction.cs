namespace Washing.Entities;

public enum TransactionType
{
    Payment,
    Deposit
}

public class Transaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Timestamp { get; set; }

    public User User { get; set; } = null!;
}
