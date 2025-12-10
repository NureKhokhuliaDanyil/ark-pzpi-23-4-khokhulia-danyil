namespace Pract1.Solid_examples;

// --- ПОГАНИЙ ПРИКЛАД ---
// Порушення LSP: Страус є птахом, але не може літати.
// Код, який очікує BadBird, зламається, якщо отримає BadOstrich.
public class BadBird
{
    public virtual void Fly()
    {
        Console.WriteLine("Птах летить...");
    }
}

public class BadOstrich : BadBird
{
    public override void Fly()
    {
        throw new NotImplementedException("Страуси не вміють літати!");
    }
}

// --- ГАРНИЙ ПРИКЛАД ---
// Виправляємо ієрархію. Базовий клас Bird не гарантує політ.
// Створюємо окремий клас для птахів, що літають.
public class GoodBird
{
    public string Name { get; set; }
}

public class GoodFlyingBird : GoodBird
{
    public void Fly()
    {
        Console.WriteLine("Птах летить високо в небі!");
    }
}

public class GoodOstrich : GoodBird
{
    // У страуса просто немає методу Fly, тому помилка неможлива.
    public void Run()
    {
        Console.WriteLine("Страус біжить швидко!");
    }
}