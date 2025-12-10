namespace Refactor.After;

public class UserService_After
{
    public async Task BuyCoin(int id, NameOfCoin coin, double amount)
    {
        var user = await GetUserByIdAsync(id);

        var coinInWallet = GetCoinFromUserOrThrow(user, coin);

        var currentPrice = coinInWallet.Price;
        // ... логіка купівлі ...
    }

    public async Task SellCoin(int id, NameOfCoin coin, double amount)
    {
        var user = await GetUserByIdAsync(id);

        var coinInWallet = GetCoinFromUserOrThrow(user, coin);

        // ... логіка продажу ...
    }

    // --- ВИРІШЕННЯ: Логіка винесена в окремий метод (Extract Method) ---
    private Coin GetCoinFromUserOrThrow(User user, NameOfCoin coinName)
    {
        var coin = user.Wallet.AmountOfCoins.FirstOrDefault(c => c.Name == coinName);
        if (coin == null)
        {
            throw new EntityNotFoundException($"Coin {coinName} not found in user's wallet");
        }
        return coin;
    }

    private Task<User> GetUserByIdAsync(int id) => Task.FromResult(new User());
}