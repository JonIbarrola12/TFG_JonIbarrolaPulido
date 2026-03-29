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
        public DbSet<Equipos> Equipos { get; set; }
        public DbSet<Clasificaciones> Clasificaciones { get; set; }
        public DbSet<LigasTemporadas> LigasTemporadas { get; set; }
        public DbSet<Jugadores> Jugadores { get; set; }
        public DbSet<JugadoresEquipos> JugadoresEquipos { get; set; } 
        public DbSet<Partidos> Partidos { get; set; }
        public DbSet<EstadisticasEquiposPartidos> EstadisticasEquiposPartidos { get; set; }
        public DbSet<EventosPartidos> EventosPartidos { get; set; }
        public DbSet<EstadisticasJugadores> EstadisticasJugadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LigasConfiguration());
            modelBuilder.ApplyConfiguration(new EquiposConfiguration());
            modelBuilder.ApplyConfiguration(new LigasTemporadasConfiguration());
            modelBuilder.ApplyConfiguration(new JugadoresConfiguration());
            modelBuilder.ApplyConfiguration(new JugadoresEquiposConfiguration());
            modelBuilder.ApplyConfiguration(new PartidosConfiguration());
            modelBuilder.ApplyConfiguration(new EstadisticasEquiposPartidosConfiguration());
            modelBuilder.ApplyConfiguration(new EventosPartidosConfiguration());
            modelBuilder.ApplyConfiguration(new EstadisticasJugadoresConfiguration());
            modelBuilder.ApplyConfiguration(new ClasificacionesConfiguration());
        }
    }
}
