namespace Refactor.Before;

public class FuethersDealService_Before
{
    public void CheckDealStatus(FuethersDeal deal, double currentPrice)
    {
        // --- ПРОБЛЕМА: Feature Envy ---
        // Сервіс рахує прибуток, використовуючи лише дані об'єкта Deal.
        // Ця логіка повинна належати самому об'єкту Deal.
        var pnl = CalculatePnl(deal, currentPrice);

        // ... логіка закриття угоди ...
    }

    private double CalculatePnl(FuethersDeal deal, double currentPrice)
    {
        if (deal.TypeOfDeal == TypeOfFuetersDeal.Long)
        {
            return (currentPrice - deal.EnterPrice) * deal.Amount * deal.Leverage;
        }
        else
        {
            return (deal.EnterPrice - currentPrice) * deal.Amount * deal.Leverage;
        }
    }
}