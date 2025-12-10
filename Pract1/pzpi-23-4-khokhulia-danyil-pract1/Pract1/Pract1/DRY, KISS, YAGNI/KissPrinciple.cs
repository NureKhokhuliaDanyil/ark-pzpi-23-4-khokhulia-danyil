using System.Globalization;

namespace Pract1;

// --- ПОГАНИЙ ПРИКЛАД ---
// Занадто складний однорядковий код, який важко читати та налагоджувати.
public class BadKissDayProvider
{
    public string GetDayName(int day)
    {
        return day < 1 || day > 7
            ? throw new ArgumentException()
            : CultureInfo.CurrentCulture.DateTimeFormat.DayNames[day == 7 ? 0 : day];
    }
}

// --- ГАРНИЙ ПРИКЛАД ---
// Проста, читабельна логіка з використанням стандартного Enum.
public class GoodKissDayProvider
{
    public string GetDayName(int day)
    {
        if (day < 1 || day > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(day), "Day must be between 1 and 7");
        }

        return ((DayOfWeek)(day % 7)).ToString();
    }
}