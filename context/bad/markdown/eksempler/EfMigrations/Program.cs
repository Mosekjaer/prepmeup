// See https://aka.ms/new-console-template for more information

using EfMigrations.Data;
using EfMigrations.Models;

List<int> ints = null!;

Console.WriteLine("Hello, World! ");

// for (var i = 0; i<200; i++) {
//     Thread thread = new Thread(DataSeeder.DoStuff);
//     thread.Start();

// }


using var context = new MyDbContext();

if (context.Departments.Count() == 0) { 
    DataSeeder.SeedData(context);
}

context.Add(new Department() {
            DepartmentName = "ECE", Location = "Katrinebjerg"
        }
);
context.SaveChanges();

Department department = context.Departments.First();
context.Remove(department);
