using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHome.Core.Entities;

namespace SportsHome.IL.Repository.EF.EntityConfigurations
{
    public class PartidosConfiguration : IEntityTypeConfiguration<Partidos>
    {
        public void Configure(EntityTypeBuilder<Partidos> builder)
        {
            builder.HasKey(p => p.PartidoId);
            builder.Property(p => p.ExternalId)
                   .IsRequired();
            builder.HasIndex(p => p.ExternalId)
                   .IsUnique();
            builder.Property(p => p.LigaId)
                   .IsRequired();
            builder.Property(p => p.Temporada)
                   .IsRequired();
            builder.Property(p => p.Fecha)
                   .IsRequired();
            builder.Property(p => p.Estado)
                   .HasMaxLength(50)
                   .IsRequired(false);
            builder.Property(p => p.Ronda)
                   .HasMaxLength(50)
                   .IsRequired(false);
            builder.Property(p => p.Arbitro)
                   .HasMaxLength(100)
                   .IsRequired(false);
            builder.Property(p => p.ZonaHoraria)
                   .HasMaxLength(50)
                   .IsRequired(false);
            builder.Property(p => p.EquipoLocalId)
                   .IsRequired();
            builder.Property(p => p.EquipoVisitanteId)
                   .IsRequired();
            builder.Property(p => p.GolesLocal)
                   .IsRequired(false);
            builder.Property(p => p.GolesVisitante)
                   .IsRequired(false);

            //Relacion con Ligas
            builder.HasOne(p => p.Liga)
                   .WithMany(l => l.Partidos)
                   .HasForeignKey(p => p.LigaId)
                   .OnDelete(DeleteBehavior.Cascade);
            // Relaciones con Equipos
            builder.HasOne(p => p.EquipoLocal)
                   .WithMany(e => e.PartidosLocal)
                   .HasForeignKey(p => p.EquipoLocalId)
                   .OnDelete(DeleteBehavior.Restrict); // Evita borrado en cascada
            builder.HasOne(p => p.EquipoVisitante)
                   .WithMany(e => e.PartidosVisitante)
                   .HasForeignKey(p => p.EquipoVisitanteId)
                   .OnDelete(DeleteBehavior.Restrict); // Evita borrado en cascada
        }
    }
}
