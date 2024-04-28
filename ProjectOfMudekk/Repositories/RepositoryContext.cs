

// using Microsoft.EntityFrameworkCore;

// namespace Repositories
// {
//     public class RepositoryContext : DbContext
//     {
//         public DbSet<Teacher> Teachers { get; set; }
//         public DbSet<HeadOfDepartment> headOfDepartments { get; set; }
//         public DbSet<HeadOfMudek> headOfMudeks { get; set; }        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             optionsBuilder.UseSqlServer("Server=CELAL_KHALILOV\\SQLEXPRESS;Database=MudekDb;TrustServerCertificate=true;User Id=sa;Password=1;");
//         }
//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);
//         }
//     }
// }