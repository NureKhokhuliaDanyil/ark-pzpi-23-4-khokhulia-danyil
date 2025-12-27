namespace Washing.Entities;

public class PromoCode
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountAmount { get; set; }
    public bool IsActive { get; set; }
}
