using ProjectAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace Project.Entities {
    public class Course {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
