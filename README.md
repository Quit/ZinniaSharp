# ZinniaSharp

ZinniaSharp is a simple (but incomplete) .NET wrapper for libzinnia. It provides managed access to the native library.

Currently, only recognizing characters is supported. Modifying or creating models, as well as more advanced features, are not available.

# Usage

In order to use ZinniaSharp, additional files that are not included are required:

- **libzinnia.dll:** The dll that SharpZinnia is wrapping. Depending on the process running ZinniaSharp, you might need to load the correct one using `NativeLibraryHelper`.
  Unless `NativeLibraryHelper` has been used to load the library, ZinniaSharp assumes that there are two corresponding dlls (`x86/libzinnia.dll` and `x64/libzinnia.dll`) relative to its own location.
- **A model:** ZinniaSharp does, at the moment, not contain bindings to modify or create models, an existing one (e.g. the tomoe one) must be supplied.

zinnia's documentation example would look like this:

```cs
using ZinniaSharp;

// Assumes that x86/libzinnia.dll and x64/libzinnia.dll exist
// and handwriting-ja.model is a valid zinnia model

// Create a recognizer
using (var recognizer = new Recognizer("handwriting-ja.model"))
{
    // Create a new character
    using (var character = new Character(300, 300))
    {
        // Add strokes
        character.AddStroke()
            .Add(51, 29)
            .Add(117, 41);

        character.AddStroke()
            .Add(99, 65)
            .Add(219, 77);

        character.AddStroke()
            .Add(27, 131)
            .Add(261, 131);

        character.AddStroke()
            .Add(129, 17)
            .Add(57, 203);

        character.AddStroke()
            .Add(111, 71)
            .Add(219, 173);

        character.AddStroke()
            .Add(81, 161)
            .Add(93, 281);

        character.AddStroke()
            .Add(99, 167)
            .Add(207, 167)
            .Add(189, 245);

        character.AddStroke()
            .Add(99, 227)
            .Add(189, 227);

        character.AddStroke()
            .Add(111, 257)
            .Add(189, 245);

        // Classify character
        var results = recognizer.Classify(character, 10);

        // Print results
        foreach (var match in results)
        {
            Console.WriteLine($"{match.Value}: {match.Score}");
        }
    }
}
```