using Washing.Entities;

namespace Washing.Extensions;

public static class UserExtensions
{
    public static bool HasSufficientBalance(this User user, decimal amount)
    {
        return user.Balance >= amount && user.IsActive;
    }
}
