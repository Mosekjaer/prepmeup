// See https://aka.ms/new-console-template for more information

using EfMigrations.Data;
using EfMigrations.Models;

public class DataSeeder
{
    // public static void DoStuff() {
    //     using var context = new MyDbContext();

    //     if (context.Departments.Count() == 0) {
    //         DataSeeder.SeedData(context);
    //     }
    // }

    public static void SeedData(MyDbContext context)
    {
        Department department = new Department() {
            DepartmentName = "ECE", Location = "Katrinebjerg"
        };
        Instructor instructor1 = new Instructor() {
            Department = department, FirstName = "Hugo", LastName = "Macedo", PhoneNumber = "11443356"
        };
        Instructor instructor2 = new Instructor() {
            Department = department, FirstName = "Henrik", LastName = "Kirk", PhoneNumber = "93403023"
        };
        Instructor instructor3 = new Instructor() {
            Department = department, FirstName = "Brian", LastName = "Danielsen", PhoneNumber = "34544334"
        };
        Course course1 = new Course() {
            Department = department, Instructors = new List<Instructor>() { instructor1, instructor2 }, Name = "Database", Duration = TimeSpan.FromHours(4)
        };
        Course course2 = new Course() {
            Department = department, Instructors = new List<Instructor>() { instructor3 }, Name = "BED", Duration = TimeSpan.FromHours(4)
        };
        Student student1 = new Student() {
            Courses = new List<Course>() { course1, course2}, FirstName = "John", LastName = "Doe", Phone = "123"
        };
        Student student2 = new Student() {
            Courses = new List<Course>() { course1}, FirstName = "Alice", LastName = "", Phone = "345"
        };
        Student student3 = new Student() {
            Courses = new List<Course>() { course2}, FirstName = "Bob", LastName = "", Phone = "532"
        };

    // 
        // Equal to:
        // context.Add(instructor1);
        // context.Add(instructor2);
        // context.Add(instructor3);

        context.Add(student1);        
        context.Add(student2);        
        context.Add(student3);

        context.SaveChanges();
    }
}