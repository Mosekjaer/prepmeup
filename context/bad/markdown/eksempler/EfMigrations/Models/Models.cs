using System.ComponentModel.DataAnnotations;

namespace EfMigrations.Models;

public class Department {
    public string DepartmentName { get; set; }
    public String Location { get; set; }

    public List<Course> Courses {get; set;}
    public List<Instructor> Employees { get; set; }
}

public class Instructor {
    public int InstructorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    public Department Department { get; set; }
    public string DepartmentId { get; set; }
    public List<Course> Courses {get; set;}
}

public class Course {
    public int CourseId { get; set; }
    public TimeSpan Duration { get; set; }    
    public string Name { get; set; }

    public List<Instructor> Instructors { get; set; }
    public Department Department {get; set;}
    public List<Student> Students { get; set; }
}

public class Student {
    public int StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }

    public List<Course> Courses { get; set; }
}