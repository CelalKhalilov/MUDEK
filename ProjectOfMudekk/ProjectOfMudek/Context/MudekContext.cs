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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=CELAL_KHALILOV\\SQLEXPRESS;Database=MudekDb3;TrustServerCertificate=true;User Id=sa;Password=1;");
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
        }
    }

}

