namespace Washing.Entities;

public enum MachineStatus
{
    Idle,
    Busy,
    Maintenance
}

public class WashingMachine
{
    public int Id { get; set; }
    public int LaundryId { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public MachineStatus Status { get; set; }

    public Laundry Laundry { get; set; } = null!;
    public ICollection<WashingSession> WashingSessions { get; set; } = new List<WashingSession>();
    public ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
}
