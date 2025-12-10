namespace Refactor.After;

public class FuethersDeal_After
{
    public double EnterPrice { get; set; }
    public double Amount { get; set; }
    public int Leverage { get; set; }
    public TypeOfFuetersDeal TypeOfDeal { get; set; }

    // --- ВИРІШЕННЯ: Метод переміщено сюди (Move Method) ---
    public double CalculatePnl(double currentPrice)
    {
        if (TypeOfDeal == TypeOfFuetersDeal.Long)
        {
            return (currentPrice - EnterPrice) * Amount * Leverage;
        }
        else
        {
            return (EnterPrice - currentPrice) * Amount * Leverage;
        }
    }
    // -----------------------------------------------------
}