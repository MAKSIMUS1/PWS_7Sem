using Microsoft.EntityFrameworkCore;
using Lab3_StudentAPI.Models;

namespace Lab3_StudentAPI.Data
{
    public class StudentsContext : DbContext
    {
        public StudentsContext(DbContextOptions<StudentsContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
