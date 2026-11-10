namespace MarksPracticeApi.DTOs.Courses;

public class ReadCourseDto
{
    public int Id { get; set; }

    public string CourseName { get; set; } = string.Empty;

    public string CourseCode { get; set; } = string.Empty;

    public int CreditHours { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}