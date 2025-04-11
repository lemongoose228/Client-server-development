using Microsoft.EntityFrameworkCore;
using Project.Entities;
using ProjectAPI.Entities;
using System.Collections.Generic;

namespace Project {
    public class AppDbContext : DbContext {
        public AppDbContext() => Database.EnsureCreated();

        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options) {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Enrollment>()
            .HasKey(e => e.EnrollmentId);
                                       

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
