
using System.Security.Claims;
using MarksPracticeApi.Data;
using MarksPracticeApi.DTOs.Students;
using MarksPracticeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarksPracticeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // Check whether the logged-in user is an Admin
    private bool IsAdmin()
    {
        return User.IsInRole("Admin");
    }

    // Get the logged-in user's ID from JWT
    private int GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(userId, out var id) ? id : 0;
    }

    // GET: api/students
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadStudentDto>>> GetStudents()
    {
        var query = _context.Students.AsNoTracking();

        // Students can only see their own records
        if (!IsAdmin())
        {
            var userId = GetCurrentUserId();

            query = query.Where(s => s.UserId == userId);
        }

        var students = await query
            .Select(s => new ReadStudentDto
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                RegistrationNumber = s.RegistrationNumber,
                CreatedAtUtc = s.CreatedAtUtc
            })
            .ToListAsync();

        return Ok(students);
    }

    // GET: api/students/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadStudentDto>> GetStudent(int id)
    {
        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
        {
            return NotFound(new { message = "Student not found." });
        }

        // Students cannot access another student's record
        if (!IsAdmin() && student.UserId != GetCurrentUserId())
        {
            return NotFound(new { message = "Student not found." });
        }

        var result = new ReadStudentDto
        {
            Id = student.Id,
            FullName = student.FullName,
            Email = student.Email,
            RegistrationNumber = student.RegistrationNumber,
            CreatedAtUtc = student.CreatedAtUtc
        };

        return Ok(result);
    }

    // POST: api/students
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReadStudentDto>> CreateStudent(
        CreateStudentDto dto)
    {
        var student = new Student
        {
            FullName = dto.FullName,
            Email = dto.Email,
            RegistrationNumber = dto.RegistrationNumber,
            CreatedAtUtc = DateTime.UtcNow
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        var result = new ReadStudentDto
        {
            Id = student.Id,
            FullName = student.FullName,
            Email = student.Email,
            RegistrationNumber = student.RegistrationNumber,
            CreatedAtUtc = student.CreatedAtUtc
        };

        return CreatedAtAction(
            nameof(GetStudent),
            new { id = student.Id },
            result);
    }

    // PUT: api/students/1
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStudent(
        int id,
        UpdateStudentDto dto)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound(new { message = "Student not found." });
        }

        student.FullName = dto.FullName;
        student.Email = dto.Email;
        student.RegistrationNumber = dto.RegistrationNumber;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/students/1
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound(new { message = "Student not found." });
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/students/1/transcript
    [HttpGet("{id:int}/transcript")]
    public async Task<ActionResult<StudentTranscriptDto>> GetStudentTranscript(
        int id)
    {
        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
        {
            return NotFound(new { message = "Student not found." });
        }

        // Students can only see their own transcript
        if (!IsAdmin() && student.UserId != GetCurrentUserId())
        {
            return NotFound(new { message = "Student not found." });
        }

        var courses = await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == id)
            .Select(e => new TranscriptCourseDto
            {
                CourseName = e.Course!.CourseName,
                CourseCode = e.Course.CourseCode,
                CreditHours = e.Course.CreditHours,
                Marks = e.Marks
            })
            .ToListAsync();

        var totalCreditHours = courses.Sum(c => c.CreditHours);

        var weightedAverage = totalCreditHours == 0
            ? 0
            : courses.Sum(c => c.Marks * c.CreditHours)
                / totalCreditHours;

        var result = new StudentTranscriptDto
        {
            StudentId = student.Id,
            StudentName = student.FullName,
            RegistrationNumber = student.RegistrationNumber,
            TotalCourses = courses.Count,
            TotalCreditHours = totalCreditHours,
            WeightedAverage = Math.Round(weightedAverage, 2),
            Courses = courses
        };

        return Ok(result);
    }
}
