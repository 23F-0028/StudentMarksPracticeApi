using System.ComponentModel.DataAnnotations;

namespace MarksPracticeApi.DTOs.Enrollments;

public class UpdateEnrollmentMarksDto
{
    [Range(0, 100)]
    public decimal Marks { get; set; }
}