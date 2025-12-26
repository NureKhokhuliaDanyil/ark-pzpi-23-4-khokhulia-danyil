namespace Washing.Entities;

public class Laundry
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string WorkingHours { get; set; } = string.Empty;

    public User Owner { get; set; } = null!;
    public ICollection<WashingMachine> WashingMachines { get; set; } = new List<WashingMachine>();
    public ICollection<WashMode> WashModes { get; set; } = new List<WashMode>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
