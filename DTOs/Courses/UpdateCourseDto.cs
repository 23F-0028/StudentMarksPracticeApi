using System.ComponentModel.DataAnnotations;

namespace MarksPracticeApi.DTOs.Courses;

public class UpdateCourseDto
{
    [Required]
    [MaxLength(100)]
    public string CourseName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string CourseCode { get; set; } = string.Empty;

    [Range(1, 4)]
    public int CreditHours { get; set; }
}