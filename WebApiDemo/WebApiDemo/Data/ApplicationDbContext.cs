using Microsoft.EntityFrameworkCore;
using WebApiDemo.Models.Entity;

namespace WebApiDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet <Employee> Emplyees { get; set; }
    }
}
