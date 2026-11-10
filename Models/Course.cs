using System.ComponentModel.DataAnnotations;

namespace MarksPracticeApi.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string CourseName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string CourseCode { get; set; } = string.Empty;

    [Range(1, 4)]
    public int CreditHours { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}