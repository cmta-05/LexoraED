using System.ComponentModel.DataAnnotations;
using LexoraED.Models;

namespace LexoraED.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Username or Email")]
    public string LoginInput { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Register as")]
    public string RegisterAs { get; set; } = LexoraRoles.Student;

    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Student ID")]
    public string? StudentIdNumber { get; set; }

    [Display(Name = "Learning Preference")]
    public string? LearningPreference { get; set; }

    [Display(Name = "Teacher / Employee ID")]
    public string? TeacherIdNumber { get; set; }

    [Display(Name = "School Name")]
    public string? SchoolName { get; set; }

    public string? Department { get; set; }

    [Display(Name = "Subject Specialization")]
    public string? SubjectSpecialization { get; set; }

    [Phone]
    [Display(Name = "Contact Number")]
    public string? ContactNumber { get; set; }
}

public class UserManagementIndexViewModel
{
    public List<UserCardViewModel> Users { get; set; } = new();
    public string? Search { get; set; }
    public string? RoleFilter { get; set; }
    public AccountStatus? StatusFilter { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

public class UserCardViewModel
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public AccountStatus AccountStatus { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? StudentIdNumber { get; set; }
    public string? TeacherIdNumber { get; set; }
    public string? SchoolName { get; set; }
}

public class UserDetailsViewModel
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public AccountStatus AccountStatus { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public StudentProfile? StudentProfile { get; set; }
    public TeacherProfile? TeacherProfile { get; set; }
    public LearningProgress? LearningProgress { get; set; }
}

public class AdminCreateViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
