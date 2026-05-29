namespace LexoraED.Models;

public class TeacherProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string TeacherIdNumber { get; set; } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string SubjectSpecialization { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}
