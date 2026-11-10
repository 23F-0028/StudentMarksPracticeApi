
using MarksPracticeApi.Data;
using MarksPracticeApi.DTOs.Courses;
using MarksPracticeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarksPracticeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/courses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadCourseDto>>> GetCourses()
    {
        var courses = await _context.Courses
            .AsNoTracking()
            .Select(c => new ReadCourseDto
            {
                Id = c.Id,
                CourseName = c.CourseName,
                CourseCode = c.CourseCode,
                CreditHours = c.CreditHours,
                CreatedAtUtc = c.CreatedAtUtc
            })
            .ToListAsync();

        return Ok(courses);
    }

    // GET: api/courses/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadCourseDto>> GetCourse(int id)
    {
        var course = await _context.Courses
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ReadCourseDto
            {
                Id = c.Id,
                CourseName = c.CourseName,
                CourseCode = c.CourseCode,
                CreditHours = c.CreditHours,
                CreatedAtUtc = c.CreatedAtUtc
            })
            .FirstOrDefaultAsync();

        if (course == null)
        {
            return NotFound(new { message = "Course not found." });
        }

        return Ok(course);
    }

    // POST: api/courses
    [HttpPost]
    public async Task<ActionResult<ReadCourseDto>> CreateCourse(
        CreateCourseDto dto)
    {
        var course = new Course
        {
            CourseName = dto.CourseName,
            CourseCode = dto.CourseCode,
            CreditHours = dto.CreditHours,
            CreatedAtUtc = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        var result = new ReadCourseDto
        {
            Id = course.Id,
            CourseName = course.CourseName,
            CourseCode = course.CourseCode,
            CreditHours = course.CreditHours,
            CreatedAtUtc = course.CreatedAtUtc
        };

        return CreatedAtAction(
            nameof(GetCourse),
            new { id = course.Id },
            result);
    }

    // PUT: api/courses/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCourse(
        int id,
        UpdateCourseDto dto)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound(new { message = "Course not found." });
        }

        course.CourseName = dto.CourseName;
        course.CourseCode = dto.CourseCode;
        course.CreditHours = dto.CreditHours;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/courses/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound(new { message = "Course not found." });
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/courses/1/stats
    [HttpGet("{id:int}/stats")]
    public async Task<ActionResult<CourseStatsDto>> GetCourseStats(int id)
    {
        var course = await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
        {
            return NotFound(new { message = "Course not found." });
        }

        var stats = await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == id)
            .GroupBy(e => e.CourseId)
            .Select(g => new
            {
                TotalStudents = g.Count(),
                AverageMarks = g.Average(e => e.Marks),
                HighestMarks = g.Max(e => e.Marks),
                LowestMarks = g.Min(e => e.Marks)
            })
            .FirstOrDefaultAsync();

        var result = new CourseStatsDto
        {
            CourseId = course.Id,
            CourseName = course.CourseName,
            TotalStudents = stats?.TotalStudents ?? 0,
            AverageMarks = stats?.AverageMarks ?? 0,
            HighestMarks = stats?.HighestMarks ?? 0,
            LowestMarks = stats?.LowestMarks ?? 0
        };

        return Ok(result);
    }
}
