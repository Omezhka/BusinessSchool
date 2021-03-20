using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessSchool.Data;
using BusinessSchool.Models;

namespace BusinessSchool.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly SchoolContext _context;

        public EnrollmentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";
            ViewData["FirstNameSortParam"] = sortOrder == "FirstName" ? "firstname_decs" : "FirstName";
            ViewData["GradeSortParam"] = sortOrder == "Grade" ? "grade_decs" : "Grade";
            ViewData["CourseSortParam"] = sortOrder == "Course" ? "course_decs" : "Course";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            IQueryable<Enrollment> enrollments = _context.Enrollments
                .Include(p => p.Student)
                .Include(p => p.Course);

            if (!String.IsNullOrEmpty(searchString))
            {
                if (Enum.TryParse<Grade>(searchString.ToUpper(), out var searchGrade))
                {
                    enrollments = enrollments.Where(s => s.Grade == searchGrade);
                }
                else
                {
                    enrollments = enrollments.Where(s => s.Student.FirstName.Contains(searchString)
                                                         || s.Student.LastName.Contains(searchString)
                                                         || s.Course.Title.Contains(searchString));
                }
            }

            switch (sortOrder)
            {
                case "lastname_desc":
                    enrollments = enrollments.OrderByDescending(s => s.Student.LastName);
                    break;
                case "FirstName":
                    enrollments = enrollments.OrderBy(s => s.Student.FirstName);
                    break;
                case "firstname_decs":
                    enrollments = enrollments.OrderByDescending(s => s.Student.FirstName);
                    break;
                case "Course":
                    enrollments = enrollments.OrderBy(s => s.Course.Title);
                    break;
                case "course_decs":
                    enrollments = enrollments.OrderByDescending(s => s.Course.Title);
                    break;
                case "Grade":
                    enrollments = enrollments.OrderBy(s => s.Grade);
                    break;
                case "grade_decs":
                    enrollments = enrollments.OrderByDescending(s => s.Grade);
                    break;
                default:
                    enrollments = enrollments.OrderBy(s => s.Student.LastName);
                    break;
            }

            int pageSize = 5;

            return View(await PaginatedList<Enrollment>.CreateAsync(enrollments.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseID");
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID");

            ViewBag.Courses = new SelectList(_context.Courses, "CourseID", "Title");
            ViewBag.Students = new SelectList(_context.Students, "ID", "LastName");
            ViewBag.Grades = new SelectList(Enum.GetValues(typeof(Grade)).Cast<Grade>());
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseID", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "LastName", enrollment.StudentID);
            ViewData["Grade"] = new SelectList(Enum.GetValues(typeof(Grade)).Cast<Grade>());

            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Courses, "CourseID", "CourseID", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentID == id);
        }
    }
}
