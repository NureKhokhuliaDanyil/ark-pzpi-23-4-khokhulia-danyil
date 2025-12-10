namespace Refactor.After;

public class FuethersDealService_After
{
    public void CheckDealStatus(FuethersDeal deal, double currentPrice)
    {
        var pnl = deal.CalculatePnl(currentPrice);
        // ... логіка закриття угоди ...
    }
}