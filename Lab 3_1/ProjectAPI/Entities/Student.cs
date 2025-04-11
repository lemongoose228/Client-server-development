using ProjectAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace Project.Entities {
    public class Student {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
