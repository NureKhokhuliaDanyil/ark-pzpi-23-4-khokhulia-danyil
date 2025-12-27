namespace Washing.Entities;

public class WashingSession
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MachineId { get; set; }
    public int ModeId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal ActualPrice { get; set; }
    public bool DoorLocked { get; set; }

    public User User { get; set; } = null!;
    public WashingMachine Machine { get; set; } = null!;
    public WashMode Mode { get; set; } = null!;
}
