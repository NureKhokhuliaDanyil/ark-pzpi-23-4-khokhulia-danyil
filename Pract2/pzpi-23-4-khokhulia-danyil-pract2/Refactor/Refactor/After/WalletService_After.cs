namespace Refactor.After;

public class WalletService_After
{
    public SeedPhrase CreateSeedPhrase()
    {
        // --- ВИРІШЕННЯ: Заміна змінної на виклик методу (Replace Temp with Query) ---
        var words = GetSeedWords();

        var random = new Random();
        var seedPhrase = new SeedPhrase { SeedPhraseValues = new List<string>() };

        for (var i = 0; i < GlobalConsts.SeedPhraseLength; i++)
        {
            var randomWord = words[random.Next(words.Count)];
            seedPhrase.SeedPhraseValues.Add(randomWord);
        }
        return seedPhrase;
    }

    // Метод-запит, що містить дані
    private List<string> GetSeedWords()
    {
        return new List<string>
        {
            "umbrella", "window", "elephant", "chair", "spaghetti", "notebook", "clover", "ocean",
            "aardvark", "chocolate", "eyebrow", "pigeon", "cup", "rose", "dragon", "cell", "fork",
            "bicycle", "lipstick", "corn", "cow", "flamingo", "ghost", "muffin", "paw", "windmill",
            "potato", "rainbow", "swamp", "whisk", "gnome", "spaceship", "wallet", "dinosaur",
            "echo", "flannel", "goblin", "hamburger", "iceberg", "jigsaw", "kaleidoscope", "lemon"
        };
    }
}