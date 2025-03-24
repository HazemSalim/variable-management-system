using Microsoft.EntityFrameworkCore;
using VariableManagementSystem.Models;

namespace VariableManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Variable> Variables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Variable>()
                .HasIndex(v => v.Identifier)
                .IsUnique();
        }
    }
}
