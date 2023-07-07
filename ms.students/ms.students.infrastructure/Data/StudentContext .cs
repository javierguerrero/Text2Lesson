using Microsoft.EntityFrameworkCore;
using ms.students.domain.Entities;

namespace ms.students.infrastructure.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasData(
                    new Student() { UserName = "mcampovero", FirstName = "Maribel", LastName = "Campovero" },
                    new Student() { UserName = "jdoe", FirstName = "John", LastName = "Doe" }
                );
        }
    }
}