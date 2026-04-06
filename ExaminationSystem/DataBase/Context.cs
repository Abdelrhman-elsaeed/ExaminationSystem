using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DataBase
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Database=ExaminationSystem;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        }

        DbSet<Course> Courses { get; set; }
    }
}
