namespace WebApplication1.Domain.DTOs;

public class StudentDto
{
    public int ID { get; set; }
    public string LastName { get; set; }
    public string FirstMidName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public int CountOfEnrollments { get; set; }
}