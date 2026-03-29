using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHome.Core.Entities;

namespace SportsHome.IL.Repository.EF.EntityConfigurations
{
    public class EstadisticasJugadoresConfiguration : IEntityTypeConfiguration<EstadisticasJugadores>
    {
        public void Configure(EntityTypeBuilder<EstadisticasJugadores> builder)
        {
            builder.HasKey(e => e.EstadisticaJugadorId);
            builder.Property(e => e.JugadorId)
                   .IsRequired();
            builder.Property(e => e.EquipoId)
                   .IsRequired();
            builder.Property(e => e.LigaId)
                   .IsRequired();
            builder.Property(e => e.Temporada)
                   .IsRequired();
            builder.Property(e => e.Apariciones).IsRequired(false);
            builder.Property(e => e.Goles).IsRequired(false);
            builder.Property(e => e.Asistencias).IsRequired(false);
            builder.Property(e => e.TarjetasAmarillas).IsRequired(false);
            builder.Property(e => e.TarjetasRojas).IsRequired(false);
            builder.Property(e => e.Minutos).IsRequired(false);
            builder.HasIndex(e => new
            {
                e.JugadorId,
                e.EquipoId,
                e.LigaId,
                e.Temporada
            }).IsUnique();

            //Relación con Jugadores (obligatoria)
            builder.HasOne(e => e.Jugador)
                   .WithMany()
                   .HasForeignKey(e => e.JugadorId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relación con Equipos (obligatoria)
            builder.HasOne(e => e.Equipo)
                   .WithMany()
                   .HasForeignKey(e => e.EquipoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relación con Ligas (obligatoria)
            builder.HasOne(e => e.Liga)
                   .WithMany()
                   .HasForeignKey(e => e.LigaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
