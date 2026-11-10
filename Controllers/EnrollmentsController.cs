
using System.Security.Claims;
using MarksPracticeApi.Data;
using MarksPracticeApi.DTOs.Enrollments;
using MarksPracticeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarksPracticeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public EnrollmentsController(AppDbContext context)
    {
        _context = context;
    }

    private bool IsAdmin()
    {
        return User.IsInRole("Admin");
    }

    private int GetCurrentUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(userId, out var id) ? id : 0;
    }

    // GET: api/enrollments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadEnrollmentDto>>> GetEnrollments()
    {
        var query = _context.Enrollments.AsNoTracking();

        // Students can only view their own enrollments.
        // Admins can view all enrollments.
        if (!IsAdmin())
        {
            var userId = GetCurrentUserId();

            query = query.Where(e =>
                e.Student != null && e.Student.UserId == userId);
        }

        var enrollments = await query
            .Select(e => new ReadEnrollmentDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                Marks = e.Marks,
                EnrolledAtUtc = e.EnrolledAtUtc
            })
            .ToListAsync();

        return Ok(enrollments);
    }

    // GET: api/enrollments/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadEnrollmentDto>> GetEnrollment(int id)
    {
        var query = _context.Enrollments
            .AsNoTracking()
            .Where(e => e.Id == id);

        // Students cannot access another student's enrollment.
        if (!IsAdmin())
        {
            var userId = GetCurrentUserId();

            query = query.Where(e =>
                e.Student != null && e.Student.UserId == userId);
        }

        var enrollment = await query
            .Select(e => new ReadEnrollmentDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                Marks = e.Marks,
                EnrolledAtUtc = e.EnrolledAtUtc
            })
            .FirstOrDefaultAsync();

        if (enrollment == null)
        {
            return NotFound(new { message = "Enrollment not found." });
        }

        return Ok(enrollment);
    }

    // POST: api/enrollments
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ReadEnrollmentDto>> CreateEnrollment(
        CreateEnrollmentDto dto)
    {
        var studentExists = await _context.Students
            .AnyAsync(s => s.Id == dto.StudentId);

        if (!studentExists)
        {
            return BadRequest(new { message = "Student does not exist." });
        }

        var courseExists = await _context.Courses
            .AnyAsync(c => c.Id == dto.CourseId);

        if (!courseExists)
        {
            return BadRequest(new { message = "Course does not exist." });
        }

        var alreadyEnrolled = await _context.Enrollments
            .AnyAsync(e =>
                e.StudentId == dto.StudentId &&
                e.CourseId == dto.CourseId);

        if (alreadyEnrolled)
        {
            return Conflict(new
            {
                message = "Student is already enrolled in this course."
            });
        }

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            Marks = dto.Marks,
            EnrolledAtUtc = DateTime.UtcNow
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        var result = new ReadEnrollmentDto
        {
            Id = enrollment.Id,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            Marks = enrollment.Marks,
            EnrolledAtUtc = enrollment.EnrolledAtUtc
        };

        return CreatedAtAction(
            nameof(GetEnrollment),
            new { id = enrollment.Id },
            result);
    }

    // PUT: api/enrollments/1/marks
    [HttpPut("{id:int}/marks")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateMarks(
        int id,
        UpdateEnrollmentMarksDto dto)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);

        if (enrollment == null)
        {
            return NotFound(new { message = "Enrollment not found." });
        }

        enrollment.Marks = dto.Marks;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/enrollments/1
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEnrollment(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);

        if (enrollment == null)
        {
            return NotFound(new { message = "Enrollment not found." });
        }

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
