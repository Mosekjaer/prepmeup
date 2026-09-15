using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Domain.DTOs;
using WebApplication1.Domain.Models;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    [HttpGet]
    public Task<List<StudentDto>> GetAllStudents(SchoolContext context)
    {
        // return context.Students.Include(s => s.Enrollments).ToListAsync(); // This gives a cycle exception, because of including the Enrollments
        return context.Students.Select(s => new StudentDto()
            {
                CountOfEnrollments = s.Enrollments.Count,
                FirstMidName = s.FirstMidName,
                LastName = s.LastName,
                EnrollmentDate = s.EnrollmentDate,
                ID = s.ID
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudent(int id, SchoolContext context)
    {
        var student = await context.Students
            .Include(s => s.Enrollments)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

        if (student == null)
        {
            return NotFound();
        }

        return new StudentDto()
        {
            CountOfEnrollments = student.Enrollments.Count,
            FirstMidName = student.FirstMidName,
            LastName = student.LastName,
            EnrollmentDate = student.EnrollmentDate,
            ID = student.ID
        };
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> CreateStudent(SchoolContext context, [FromBody] CreateStudentDto student)
    {
        //map from createStudentDto to Student
        var createdStudent = new Student()
        {
            FirstMidName = student.FirstMidName,
            LastName = student.LastName,
            EnrollmentDate = student.EnrollmentDate
        };

        context.Students.Add(createdStudent);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.ID }, createdStudent);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudent(int id, SchoolContext context)
    {
        var student = await context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        context.Students.Remove(student);
        await context.SaveChangesAsync();

        return Ok();
    }
}