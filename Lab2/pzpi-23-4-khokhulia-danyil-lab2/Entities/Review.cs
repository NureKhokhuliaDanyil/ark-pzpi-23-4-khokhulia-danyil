namespace Washing.Entities;

public class Review
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int LaundryId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

    public User User { get; set; } = null!;
    public Laundry Laundry { get; set; } = null!;
}
