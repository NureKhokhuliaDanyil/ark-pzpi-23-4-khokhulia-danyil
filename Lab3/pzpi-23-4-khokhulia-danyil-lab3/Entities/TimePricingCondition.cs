namespace Washing.Entities;

public class TimePricingCondition
{
    public int Id { get; set; }
    public TimeSpan TimeStart { get; set; }
    public TimeSpan TimeEnd { get; set; }
    public decimal Multiplier { get; set; }
    public bool IsActive { get; set; }
}
