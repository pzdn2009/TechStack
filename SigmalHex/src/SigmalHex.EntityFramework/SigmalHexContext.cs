using Microsoft.EntityFrameworkCore;
using SigmalHex.Domain.KBContext.Entities;

namespace SigmalHex.EntityFramework
{
    public class SigmalHexContext : DbContext
    {
        public SigmalHexContext(DbContextOptions<SigmalHexContext> options) : base(options)
        {
        }

        public DbSet<Knowledge> Knowledges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Knowledge>().ToTable("Knowledge");
        }
    }
}
