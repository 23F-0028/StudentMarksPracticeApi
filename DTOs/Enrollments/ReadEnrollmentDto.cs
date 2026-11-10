namespace MarksPracticeApi.DTOs.Enrollments;

public class ReadEnrollmentDto
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public decimal Marks { get; set; }

    public DateTime EnrolledAtUtc { get; set; }
}