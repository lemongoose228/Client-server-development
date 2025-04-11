using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project;
using Project.Entities;
using ProjectAPI.DTOs;

namespace ProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses() {
            return await _context.Courses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseById(int id) {
            var course = await _context.Courses.FindAsync(id);

            if (course == null) {
                return BadRequest("Курс не найден");
            }

            return course;
        }

        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(CourseResponse course) {
            var newCourse = new Course {
                CourseName = course.CourseName,
                Description = course.Description
            };

            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, CourseResponse course) {
            var foundCourse = await _context.Courses.FindAsync(id);

            if (foundCourse == null) {
                return BadRequest("Курс не найден");
            }

            foundCourse.CourseName = course.CourseName;
            foundCourse.Description = course.Description;

            await _context.SaveChangesAsync();

            return Ok(foundCourse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id) {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) {
                return BadRequest("Курс не найден");
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}/students")]
        public async Task<ActionResult<IEnumerable<StudentResponse>>> GetCourseStudents(int id) {
            var course = await _context.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null) {
                return BadRequest("Курс не найден");
            }

            return course.Enrollments.Select(e => new StudentResponse {
                FirstName = e.Student.FirstName,
                LastName = e.Student.LastName,
                Email = e.Student.Email,
                DateOfBirth = e.Student.DateOfBirth,

            }).ToList();
        }
    }
}
