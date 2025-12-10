namespace Refactor.Before;

public class WalletService_Before
{
    public SeedPhrase CreateSeedPhrase()
    {
        // --- ПРОБЛЕМА: Велика тимчасова змінна захаращує метод ---
        var words = new List<string>
        {
            "umbrella", "window", "elephant", "chair", "spaghetti", "notebook", "clover", "ocean",
            "aardvark", "chocolate", "eyebrow", "pigeon", "cup", "rose", "dragon", "cell", "fork",
            "bicycle", "lipstick", "corn", "cow", "flamingo", "ghost", "muffin", "paw", "windmill",
            "potato", "rainbow", "swamp", "whisk", "gnome", "spaceship", "wallet", "dinosaur",
            "echo", "flannel", "goblin", "hamburger", "iceberg", "jigsaw", "kaleidoscope", "lemon"
        };
        // ----------------------------------------------------------

        var random = new Random();
        var seedPhrase = new SeedPhrase { SeedPhraseValues = new List<string>() };

        for (var i = 0; i < GlobalConsts.SeedPhraseLength; i++)
        {
            var randomWord = words[random.Next(words.Count)];
            seedPhrase.SeedPhraseValues.Add(randomWord);
        }
        return seedPhrase;
    }
}