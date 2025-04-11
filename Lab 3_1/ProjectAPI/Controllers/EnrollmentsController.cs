using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project;
using Project.Entities;
using ProjectAPI.DTOs;
using ProjectAPI.Entities;

namespace ProjectAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase {
        private readonly AppDbContext _context;

        public EnrollmentsController(AppDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments() {
            return await _context.Enrollments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EntrollmentsResponse>> GetEnrollmentById(int id) {
            var enrollment = await _context.Enrollments
                .Include(e => e.Student) 
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (enrollment == null) {
                return BadRequest("Курс не найден");
            }

            return new EntrollmentsResponse {
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                Grade = enrollment.Grade
            };
        }

        [HttpPost]
        public async Task<ActionResult<Enrollment>> CreateEnrollment(EntrollmentsResponse enrollment) {
            if (!_context.Students.Any(s => s.StudentId == enrollment.StudentId) ||
                !_context.Courses.Any(c => c.CourseId == enrollment.CourseId)) {
                return BadRequest("Не верный StudentId или CourseId");
            }

            var newEntrollment = new Enrollment {
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                EnrollmentDate = DateTime.Now,
                Grade = enrollment.Grade
            };

            _context.Enrollments.Add(newEntrollment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrollment(int id, EntrollmentsResponse enrollment) {
            var foundEnrollment = await _context.Enrollments.FindAsync(id);

            if (foundEnrollment == null) {
                return BadRequest("Зачисление не найдено");
            }

            foundEnrollment.StudentId = enrollment.StudentId;
            foundEnrollment.CourseId = enrollment.CourseId;
            foundEnrollment.Grade = enrollment.Grade;

            await _context.SaveChangesAsync();

            return Ok(foundEnrollment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id) {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) {
                return BadRequest("Зачисление не найдено");
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollmentsByStudent(int studentId) {
            return await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollmentsByCourse(int courseId) {
            return await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .ToListAsync();
        }

        [HttpGet("student/{studentId}/course/{courseId}")]
        public async Task<ActionResult<Enrollment>> GetEnrollmentByStudentAndCourse(int studentId, int courseId) {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment == null) {
                return BadRequest("Зачисление не найдено");
            }

            return enrollment;
        }

        [HttpGet("grades/student/{studentId}")]
        public async Task<ActionResult<double>> GetStudentGPA(int studentId) {
            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == studentId && e.Grade.HasValue)
                .ToListAsync();

            if (enrollments.Count == 0) {
                return BadRequest("У студента нет оценок");
            }

            double totalGradePoints = enrollments.Sum(e => e.Grade.Value);
            double gpa = totalGradePoints / enrollments.Count;

            return gpa;
        }

        [HttpGet("grades/course/{courseId}")]
        public async Task<ActionResult<double>> GetCourseAverageGrade(int courseId) {
            var enrollments = await _context.Enrollments
                 .Where(e => e.CourseId == courseId && e.Grade.HasValue)
                 .ToListAsync();

            if (enrollments.Count == 0) {
                return NotFound("У курса нет оценок");
            }
            double totalGradePoints = enrollments.Sum(e => e.Grade.Value);
            double averageGrade = totalGradePoints / enrollments.Count;

            return averageGrade;
        }

        [HttpGet("ungraded")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetUngradedEnrollments() {
            return await _context.Enrollments
                .Where(e => e.Grade == null)
                .ToListAsync();
        }
    }
}
