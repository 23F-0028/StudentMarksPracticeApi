using System.ComponentModel.DataAnnotations;

namespace MarksPracticeApi.DTOs.Enrollments;

public class CreateEnrollmentDto
{
    [Range(1, int.MaxValue)]
    public int StudentId { get; set; }

    [Range(1, int.MaxValue)]
    public int CourseId { get; set; }

    [Range(0, 100)]
    public decimal Marks { get; set; }
}