using System.ComponentModel.DataAnnotations;

namespace MarksPracticeApi.DTOs.Students;

public class UpdateStudentDto
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string RegistrationNumber { get; set; } = string.Empty;
}