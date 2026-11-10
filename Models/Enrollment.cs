using System.ComponentModel.DataAnnotations;

namespace MarksPracticeApi.Models;

public class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    [Range(0, 100)]
    public decimal Marks { get; set; }

    public DateTime EnrolledAtUtc { get; set; } = DateTime.UtcNow;

    public Student? Student { get; set; }

    public Course? Course { get; set; }
}