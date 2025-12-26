namespace Washing.Entities;

public class WashMode
{
    public int Id { get; set; }
    public int LaundryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }

    public Laundry Laundry { get; set; } = null!;
    public ICollection<WashingSession> WashingSessions { get; set; } = new List<WashingSession>();
}
