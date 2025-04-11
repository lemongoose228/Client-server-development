using Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Entities {
    public class Enrollment {
        [Key]
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int? Grade { get; set; } 

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
