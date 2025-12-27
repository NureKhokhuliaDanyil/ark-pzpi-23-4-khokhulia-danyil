namespace Washing.Interfaces;

public interface IPaymentService
{
    Task<decimal> GetBalanceAsync(int userId);
    Task ApplyPromoCodeAsync(int userId, string promoCode);
    Task ProcessPaymentAsync(int userId, decimal amount);
}
