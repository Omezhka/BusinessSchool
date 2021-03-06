using BusinessSchool.Data;
using BusinessSchool.Models;
using BusinessSchool.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessSchool.Controllers
{
       
    public class HomeController : Controller
    {
        private readonly SchoolContext _context;

        public HomeController(SchoolContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> About()
        {
            IQueryable<EnrollmentDateGroup> data =
                from student in _context.Students
                group student by student.EnrollmentDate into dateGroup 
                select new EnrollmentDateGroup()
                {
                    EnrollmentDate = dateGroup.Key,
                    StudentCount = dateGroup.Count()
                };

            Dictionary<string, int> coursesRating = new Dictionary<string, int>();

            foreach (var course in _context.Courses)
            {
                coursesRating.Add(course.Title, 0);
            }

            foreach (var enrollment in _context.Enrollments)
            {
                coursesRating[enrollment.Course.Title]++;
            }

            ViewData["coursesRating"] = coursesRating;

            Dictionary<Student, int> studentsRating = new Dictionary<Student, int>();

            foreach (var student in _context.Students)
            {
                studentsRating.Add(student, 0);
            }

            foreach (var enrollment in _context.Enrollments)
            {
                studentsRating[enrollment.Student]++;
            }

            ViewData["studentsRating"] = studentsRating;

            return View(await data.AsNoTracking().ToListAsync());

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
