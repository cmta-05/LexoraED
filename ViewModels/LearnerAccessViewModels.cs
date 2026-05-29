using System.ComponentModel.DataAnnotations;
using LexoraED.Models;

namespace LexoraED.ViewModels;

public class LearnerLoginViewModel
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class LearnerRegistrationViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public LearnerRole Role { get; set; } = LearnerRole.Student;
}
