using Microsoft.EntityFrameworkCore;
using SportsHome.Core.Entities;
using SportsHome.IL.Repository.EF.EntityConfigurations;

namespace SportsHome.IL.Repository.EF
{
    public class SportsHomeContext : DbContext
    {
        public SportsHomeContext(DbContextOptions<SportsHomeContext> options)
            : base(options)
        {
        }
        public DbSet<Ligas> Ligas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LigasConfiguration());
        }
    }
}
