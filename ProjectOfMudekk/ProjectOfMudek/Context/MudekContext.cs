using Entities.Entities.Models;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProjectOfMudek.Context
{
    public class MudekContext : DbContext
    {
        public MudekContext(DbContextOptions<MudekContext> options) : base(options)
        {
        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<HeadOfDepartment> headOfDepartments { get; set; }
        public DbSet<HeadOfMudek> headOfMudeks { get; set; }
        public DbSet<AcademicUnit> academicUnits { get; set; }
        public DbSet<Faculty> faculties { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<LearningOutcomes> learningOutcomess { get; set; }
        public DbSet<AssessmentTool> assessmentTools { get; set; }
        public DbSet<SubAssessmentTool> subAssessmentTools { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<Question> questions { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=CELAL_KHALILOV\\SQLEXPRESS;Database=MudekDb;TrustServerCertificate=true;User Id=sa;Password=1;");
            // optionsBuilder.UseSqlServer("Server=MAYBEONEDAY\\SQLEXPRESS;Database=MudekDb;TrustServerCertificate=true;User Id=sa;Password=HarunGcl2312.");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Faculty>()
                .HasOne(ba => ba.AcademicUnit)
                .WithMany(a => a.FacultyList)
                .HasForeignKey(a => a.AcademicUnitId);

            modelBuilder.Entity<Department>()
                .HasOne(ba => ba.Faculty)
                .WithMany(a => a.DepartmentList)
                .HasForeignKey(a => a.FacultyId);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Student)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.StudentId);

        }
    }

}

