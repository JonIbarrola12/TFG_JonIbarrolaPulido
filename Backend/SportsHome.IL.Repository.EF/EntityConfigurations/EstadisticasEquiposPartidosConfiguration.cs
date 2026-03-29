using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHome.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.IL.Repository.EF.EntityConfigurations
{
    public class EstadisticasEquiposPartidosConfiguration : IEntityTypeConfiguration<EstadisticasEquiposPartidos>
    {
        public void Configure(EntityTypeBuilder<EstadisticasEquiposPartidos> builder)
        {
            builder.HasKey(e => e.EstadisticaPartidoEquipoId);
            builder.Property(e => e.PartidoId)
                   .IsRequired();
            builder.Property(e => e.EquipoId)
                   .IsRequired();
            builder.Property(e => e.TirosAPuerta)
                   .IsRequired(false);
            builder.Property(e => e.TirosFuera)
                   .IsRequired(false);
            builder.Property(e => e.Posesion)
                   .HasPrecision(5, 2) // Ej: 65.50
                   .IsRequired(false);
            builder.Property(e => e.Faltas)
                   .IsRequired(false);
            builder.Property(e => e.Corners)
                   .IsRequired(false);
            builder.Property(e => e.TarjetasAmarillas)
                   .IsRequired(false);
            builder.Property(e => e.TarjetasRojas)
                   .IsRequired(false);
            builder.HasIndex(e => new { e.PartidoId, e.EquipoId })
                   .IsUnique();

            // Relación con Partido
            builder.HasOne(e => e.Partido)
                   .WithMany(p => p.Estadisticas)
                   .HasForeignKey(e => e.PartidoId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relación con Equipo
            builder.HasOne(e => e.Equipo)
                   .WithMany() // normalmente no necesitas navegación inversa
                   .HasForeignKey(e => e.EquipoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
