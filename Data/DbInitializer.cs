using BusinessSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessSchool.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();
            //просмотр данных в табл student
            if (context.Students.Any())
            {
                return; //заполнена
            }

            var students = new Student[]
            {
                new Student{FirstName="Иван Иванович", LastName="Иванов", EnrollmentDate=DateTime.Parse("2020-09-01")},
                new Student{FirstName="Николай Алексеевич", LastName="Амосов", EnrollmentDate=DateTime.Parse("2018-09-01")},
                new Student{FirstName="Артур", LastName="Никифоров", EnrollmentDate=DateTime.Parse("2020-09-01")},
                new Student{FirstName="Ника", LastName="Александрова", EnrollmentDate=DateTime.Parse("2019-09-01")},
                new Student{FirstName="Ян Андерс", LastName="Ли", EnrollmentDate=DateTime.Parse("2019-09-01")},
                new Student{FirstName="Максим Максимович", LastName="Жуков", EnrollmentDate=DateTime.Parse("2018-09-01")},
                new Student{FirstName="Елена Викторовна", LastName="Морозова", EnrollmentDate=DateTime.Parse("2020-09-01")},
                new Student{FirstName="Аркадий Алексеевич", LastName="Слован", EnrollmentDate=DateTime.Parse("2019-09-01")}
            };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();

            var courses = new Course[]
            {
                new Course{CourseID = 1050, Title = "VR и дизайн персонажей", Credits = 3},
                new Course{CourseID = 4022, Title = "Микроэкономика", Credits = 3},
                new Course{CourseID = 4041, Title = "Макроэкономика и международные отношения", Credits = 3},
                new Course{CourseID = 1045, Title = "Защита корпоративных систем", Credits = 4},
                new Course{CourseID = 3141, Title = "Анализ данных", Credits = 4},
                new Course{CourseID = 2021, Title = "Глубокое обучение и нейросети", Credits = 3},
                new Course{CourseID = 2042, Title = "Прикладные информационные системы", Credits = 4}
            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment{StudentID = 1, CourseID = 1050, Grade = Grade.A},
                new Enrollment{StudentID = 1, CourseID = 4022, Grade = Grade.C},
                new Enrollment{StudentID = 1, CourseID = 4041, Grade = Grade.B},
                new Enrollment{StudentID = 2, CourseID = 1045, Grade = Grade.B},
                new Enrollment{StudentID = 2, CourseID = 3141, Grade = Grade.F},
                new Enrollment{StudentID = 2, CourseID = 2021, Grade = Grade.F},
                new Enrollment{StudentID = 3, CourseID = 1050},
                new Enrollment{StudentID = 4, CourseID = 1050},
                new Enrollment{StudentID = 4, CourseID = 4022, Grade = Grade.F},
                new Enrollment{StudentID = 5, CourseID = 4041, Grade = Grade.C},
                new Enrollment{StudentID = 6, CourseID = 1045},
                new Enrollment{StudentID = 7, CourseID = 3141, Grade = Grade.A},
            };

            foreach (Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
            context.SaveChanges();
        }
        
    }
}
