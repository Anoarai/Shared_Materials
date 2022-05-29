using Microsoft.EntityFrameworkCore;
using Template_for_ASP.NET___entity_framework.Models;

namespace Template_for_ASP.NET___entity_framework.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Log
            modelBuilder.Entity<Log>().HasKey(l => l.Id);
            modelBuilder.Entity<Log>().Property(l => l.Method).IsRequired();
            modelBuilder.Entity<Log>().Property(l => l.FunctionName).IsRequired();
            modelBuilder.Entity<Log>().Property(l => l.Paremeters).IsRequired(false);
        }
    }
}
