namespace Washing.Entities;

public class MaintenanceLog
{
    public int Id { get; set; }
    public int MachineId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public decimal? Cost { get; set; }

    public WashingMachine Machine { get; set; } = null!;
}
