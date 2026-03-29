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
    public class ClasificacionesConfiguration : IEntityTypeConfiguration<Clasificaciones>
    {
        public void Configure(EntityTypeBuilder<Clasificaciones> builder)
        {
            builder.HasKey(c => c.ClasificacionId);
            builder.Property(c => c.LigaId)
                   .IsRequired();
            builder.Property(c => c.EquipoId)
                   .IsRequired();
            builder.Property(c => c.Temporada)
                   .IsRequired();
            builder.Property(c => c.Jugados).IsRequired();
            builder.Property(c => c.Ganados).IsRequired();
            builder.Property(c => c.Empatados).IsRequired();
            builder.Property(c => c.Perdidos).IsRequired();
            builder.Property(c => c.GolesAFavor).IsRequired();
            builder.Property(c => c.GolesEnContra).IsRequired();
            builder.Property(c => c.Puntos).IsRequired();
            builder.Property(c => c.Posicion).IsRequired();
            builder.HasIndex(c => new
            {
                c.LigaId,
                c.EquipoId,
                c.Temporada
            }).IsUnique();

            //Relación con Liga (obligatoria)
            builder.HasOne(c => c.Liga)
                   .WithMany(l => l.Clasificaciones)
                   .HasForeignKey(c => c.LigaId)
                   .OnDelete(DeleteBehavior.Restrict);

            //Relación con Equipo (obligatoria)
            builder.HasOne(c => c.Equipo)
                   .WithMany(e => e.Clasificaciones)
                   .HasForeignKey(c => c.EquipoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Índice útil para ordenar clasificación
            builder.HasIndex(c => new
            {
                c.LigaId,
                c.Temporada,
                c.Posicion
            });
        }
    }
}
