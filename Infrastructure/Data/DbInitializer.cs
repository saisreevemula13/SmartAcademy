using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.Students.Any())
            {
                var students = new List<Student>
                {
                    new Student { Name = "Piyush", Email = "piyush@example.com", PasswordHash = "hashed123", Role = "Student" },
                    new Student { Name = "Yukta", Email = "yukta@example.com", PasswordHash = "hashed456", Role = "Student" }
                };

                context.Students.AddRange(students);
                context.SaveChanges();
            }

            // 3️⃣ Seed Courses
            if (!context.Courses.Any())
            {
                var courses = new List<Course>
                {
                    new Course { Name = "Sql", Description = "Learn the basics of sql programming" },
                    new Course { Name = "ASP.NET Core Web API", Description = "Build REST APIs with .NET 8" },
                    new Course { Name = "Angular 18 for Beginners", Description = "Introduction to modern Angular" }
                };

                context.Courses.AddRange(courses);
                context.SaveChanges();
            }

            // 4️⃣ Seed Enrollments (only if Students & Courses exist)
            if (!context.Enrollments.Any())
            {
                var student1 = context.Students.FirstOrDefault(s => s.Name == "Piyush");
                var student2 = context.Students.FirstOrDefault(s => s.Name == "Yukta");

                var course1 = context.Courses.FirstOrDefault(c => c.Name == "Sql");
                var course2 = context.Courses.FirstOrDefault(c => c.Name == "ASP.NET Core Web API");

                if (student1 != null && student2 != null && course1 != null && course2 != null)
                {
                    var enrollments = new List<Enrollment>
                    {
                        new Enrollment { StudentId = student1.Id, CourseId = course1.Id },
                        new Enrollment { StudentId = student1.Id, CourseId = course2.Id },
                        new Enrollment { StudentId = student2.Id, CourseId = course1.Id }
                    };

                    context.Enrollments.AddRange(enrollments);
                    context.SaveChanges();
                }
            }
        }
    }
}
