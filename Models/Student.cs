
using System.ComponentModel.DataAnnotations;

namespace MarksPracticeApi.Models;

public class Student
{
    public int Id { get; set; }

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

    // Optional relationship with login account
    public int? UserId { get; set; }

    // Navigation property
    public User? User { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
