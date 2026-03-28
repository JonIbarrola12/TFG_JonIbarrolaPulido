using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHome.Core.Entities;

namespace SportsHome.IL.Repository.EF.EntityConfigurations
{
    public class JugadoresEquiposConfiguration : IEntityTypeConfiguration<JugadoresEquipos>
    {
        public void Configure(EntityTypeBuilder<JugadoresEquipos> builder)
        {
            // Clave primaria
            builder.HasKey(je => je.JugadorEquipoId);
            builder.Property(je => je.JugadorId)
                   .IsRequired();
            builder.Property(je => je.EquipoId)
                   .IsRequired();
            builder.Property(je => je.Temporada)
                   .IsRequired();
            builder.HasIndex(je => new { je.JugadorId, je.EquipoId, je.Temporada })
                   .IsUnique();
            // Relacion con Jugadores
            builder.HasOne(je => je.Jugador)
                   .WithMany(j => j.JugadoresEquipos)
                   .HasForeignKey(je => je.JugadorId)
                   .OnDelete(DeleteBehavior.Cascade);
            //Relacion con Equipos
            builder.HasOne(je => je.Equipo)
                   .WithMany(e => e.JugadoresEquipos)
                   .HasForeignKey(je => je.EquipoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
