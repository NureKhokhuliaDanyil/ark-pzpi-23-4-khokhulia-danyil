namespace Refactor.Before;

public class UserService_Before
{
    public async Task BuyCoin(int id, NameOfCoin coin, double amount)
    {
        var user = await GetUserByIdAsync(id);

        // --- ПРОБЛЕМА: Дублювання коду пошуку та валідації ---
        var coinInWallet = user.Wallet.AmountOfCoins.FirstOrDefault(c => c.Name == coin);

        if (coinInWallet == null)
        {
            throw new EntityNotFoundException($"Coin {coin} not found in user's wallet");
        }

        var currentPrice = coinInWallet.Price;
        // ... логіка купівлі (зміна балансу, збереження) ...
    }

    public async Task SellCoin(int id, NameOfCoin coin, double amount)
    {
        var user = await GetUserByIdAsync(id);

        var coinInWallet = user.Wallet.AmountOfCoins.FirstOrDefault(c => c.Name == coin);

        if (coinInWallet == null)
        {
            throw new EntityNotFoundException($"Coin {coin} not found in user's wallet");
        }

        // ... логіка продажу ...
    }

    private Task<User> GetUserByIdAsync(int id) => Task.FromResult(new User()); // Stub
}