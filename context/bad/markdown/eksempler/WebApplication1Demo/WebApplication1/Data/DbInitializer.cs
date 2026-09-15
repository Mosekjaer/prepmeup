using WebApplication1.Domain.Models;

namespace WebApplication1.Data;

public static class DbInitializer
{
    public static void Initialize(SchoolContext context)
    {
        // Look for any students.
        if (context.Students.Any())
        {
            return; // DB has been seeded
        }

        var students = new Student[]
        {
            new Student
            {
                FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2019-09-01")
            },
            new Student
            {
                FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2017-09-01")
            },
            new Student { FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2018-09-01") },
            new Student
            {
                FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2017-09-01")
            },
            new Student { FirstMidName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2017-09-01") },
            new Student { FirstMidName = "Peggy", LastName = "Justice", EnrollmentDate = DateTime.Parse("2016-09-01") },
            new Student { FirstMidName = "Laura", LastName = "Norman", EnrollmentDate = DateTime.Parse("2018-09-01") },
            new Student { FirstMidName = "Nino", LastName = "Olivetto", EnrollmentDate = DateTime.Parse("2019-09-01") }
        };

        context.Students.AddRange(students);
        context.SaveChanges();

        var courses = new Course[]
        {
            new Course { CourseID = 1050, Title = "Chemistry", Credits = 3 },
            new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3 },
            new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3 },
            new Course { CourseID = 1045, Title = "Calculus", Credits = 4 },
            new Course { CourseID = 3141, Title = "Trigonometry", Credits = 4 },
            new Course { CourseID = 2021, Title = "Composition", Credits = 3 },
            new Course { CourseID = 2042, Title = "Literature", Credits = 4 }
        };

        context.Courses.AddRange(courses);
        context.SaveChanges();

        var enrollments = new Enrollment[]
        {
            // new Enrollment { StudentID = 2, CourseID = 3141, Grade = Grade.F }, // The student ID is hard-coded
            new Enrollment { StudentID = students[0].ID, CourseID = 1050, Grade = Grade.A }, // The student ID is from the array of students
            new Enrollment { StudentID = students[0].ID, CourseID = 4022, Grade = Grade.C },
            new Enrollment { StudentID = students[0].ID, CourseID = 4041, Grade = Grade.B },
            new Enrollment { StudentID = students[1].ID, CourseID = 1045, Grade = Grade.B },
            new Enrollment { StudentID = students[1].ID, CourseID = 3141, Grade = Grade.F },
            new Enrollment { StudentID = students[1].ID, CourseID = 2021, Grade = Grade.F },
            new Enrollment { StudentID = students[2].ID, CourseID = 1050 },
            new Enrollment { StudentID = students[3].ID, CourseID = 1050 },
            new Enrollment { StudentID = students[3].ID, CourseID = 4022, Grade = Grade.F },
            new Enrollment { StudentID = students[4].ID, CourseID = 4041, Grade = Grade.C },
            new Enrollment { StudentID = students[5].ID, CourseID = 1045 },
            new Enrollment { StudentID = students[6].ID, CourseID = 3141, Grade = Grade.A },
        };

        context.Enrollments.AddRange(enrollments);
        context.SaveChanges();
    }
}