namespace LexoraED.Models;

public class StudentProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string StudentIdNumber { get; set; } = string.Empty;
    public string? LearningPreference { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
