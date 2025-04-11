using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project;
using Project.Entities;
using ProjectAPI.DTOs;

namespace ProjectAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents() {
            return await _context.Students.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id) {
            var student = await _context.Students.FindAsync(id);

            if (student == null) {
                return BadRequest("Студент не найден");
            }

            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(StudentResponse student) {
            var newStudent = new Student {
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email
            };

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            return Ok(newStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, StudentResponse student) {
            var foundStudent = await _context.Students.FindAsync(id);

            if (foundStudent == null) {
                return BadRequest("Студент не найден");
            }

            foundStudent.FirstName = student.FirstName;
            foundStudent.LastName = student.LastName;
            foundStudent.DateOfBirth = student.DateOfBirth;
            foundStudent.Email = student.Email;

            await _context.SaveChangesAsync();

            return Ok(foundStudent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id) {
            var student = await _context.Students.FindAsync(id);
            if (student == null) {
                return BadRequest("Студент не найден"); 
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}/courses")]
        public async Task<ActionResult<IEnumerable<CourseResponse>>> GetStudentCourses(int id) {
            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null) {
                return BadRequest("Студент не найден"); 
            }

            return student.Enrollments.Select(
                e => new CourseResponse {
                    CourseName = e.Course.CourseName,
                    Description = e.Course.Description,
                    
                }).ToList();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Student>>> SearchStudents(string firstName, string lastName) {
            IQueryable<Student> query = _context.Students;

            if (!string.IsNullOrEmpty(firstName)) {
                query = query.Where(s => s.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName)) {
                query = query.Where(s => s.LastName.Contains(lastName));
            }

            return await query.ToListAsync();
        }
    }
}
