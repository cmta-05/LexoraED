namespace LexoraED.Models;

public class TeacherProfile
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public string Department { get; set; } = "English Education";
    public string Specialization { get; set; } = "Microlearning Instruction";

    public Learner Learner { get; set; } = null!;
}
