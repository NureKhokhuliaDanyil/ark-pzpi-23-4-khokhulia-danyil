namespace Pract1;

// --- ПОГАНИЙ ПРИКЛАД ---
// Логіка розрахунку податку дублюється.
public class BadSalaryPrinter
{
    public void PrintManagerSalary(decimal baseSalary)
    {
        decimal tax = baseSalary * 0.18m;
        decimal netSalary = baseSalary - tax;
        Console.WriteLine($"Зарплата менеджера: {netSalary} грн");
    }

    public void PrintDeveloperSalary(decimal baseSalary)
    {
        decimal tax = baseSalary * 0.18m;
        decimal netSalary = baseSalary - tax;
        Console.WriteLine($"Зарплата розробника: {netSalary} грн");
    }
}

// --- ГАРНИЙ ПРИКЛАД ---
// Логіка розрахунку централізована в одному методі.
public class GoodSalaryPrinter
{
    public void PrintManagerSalary(decimal baseSalary)
    {
        decimal netSalary = CalculateNetSalary(baseSalary);
        Console.WriteLine($"Зарплата менеджера: {netSalary} грн");
    }

    public void PrintDeveloperSalary(decimal baseSalary)
    {
        decimal netSalary = CalculateNetSalary(baseSalary);
        Console.WriteLine($"Зарплата розробника: {netSalary} грн");
    }

    private decimal CalculateNetSalary(decimal baseSalary)
    {
        const decimal TaxRate = 0.18m;
        return baseSalary - (baseSalary * TaxRate);
    }
}