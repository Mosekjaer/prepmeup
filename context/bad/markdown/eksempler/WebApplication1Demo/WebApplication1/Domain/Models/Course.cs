using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Domain.Models;

public class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CourseID { get; set; }

    public string Title { get; set; }
    public int Credits { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
}