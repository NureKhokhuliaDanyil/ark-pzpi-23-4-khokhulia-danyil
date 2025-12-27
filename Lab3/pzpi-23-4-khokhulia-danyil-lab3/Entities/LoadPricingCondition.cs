namespace Washing.Entities;

public class LoadPricingCondition
{
    public int Id { get; set; }
    public decimal LoadThreshold { get; set; }
    public decimal Multiplier { get; set; }
    public bool IsActive { get; set; }
}
