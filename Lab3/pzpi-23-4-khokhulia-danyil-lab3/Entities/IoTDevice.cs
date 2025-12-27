namespace Washing.Entities;

public enum IoTDeviceStatus
{
    Online,
    Offline
}

public class IoTDevice
{
    public int Id { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public int? MachineId { get; set; }
    public DateTime LastPing { get; set; }
    public IoTDeviceStatus Status { get; set; }

    public WashingMachine? Machine { get; set; }
}
