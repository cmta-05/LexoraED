namespace LexoraED.Models;

public enum DifficultyLevel
{
    Beginner,
    Intermediate,
    Advanced
}

public static class DifficultyLevelExtensions
{
    public static DifficultyLevel Normalize(DifficultyLevel level) => level;

    public static int TierOrder(this DifficultyLevel level) => level switch
    {
        DifficultyLevel.Beginner => 0,
        DifficultyLevel.Intermediate => 1,
        _ => 2
    };

    public static DifficultyLevel? ParseLegacy(string value) => value switch
    {
        "Easy" => DifficultyLevel.Beginner,
        "Medium" => DifficultyLevel.Intermediate,
        "Hard" => DifficultyLevel.Advanced,
        _ => Enum.TryParse<DifficultyLevel>(value, out var l) ? l : null
    };
}
