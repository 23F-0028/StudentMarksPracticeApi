namespace MarksPracticeApi.DTOs.Students;

public class StudentTranscriptDto
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string RegistrationNumber { get; set; } = string.Empty;

    public int TotalCourses { get; set; }

    public int TotalCreditHours { get; set; }

    public decimal WeightedAverage { get; set; }

    public List<TranscriptCourseDto> Courses { get; set; } = new();
}

public class TranscriptCourseDto
{
    public string CourseName { get; set; } = string.Empty;

    public string CourseCode { get; set; } = string.Empty;

    public int CreditHours { get; set; }

    public decimal Marks { get; set; }
}