namespace MarksPracticeApi.DTOs.Courses;

public class CourseStatsDto
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = string.Empty;

    public int TotalStudents { get; set; }

    public decimal AverageMarks { get; set; }

    public decimal HighestMarks { get; set; }

    public decimal LowestMarks { get; set; }
}