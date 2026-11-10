namespace MarksPracticeApi.DTOs.Students;

public class ReadStudentDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string RegistrationNumber { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}