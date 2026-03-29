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
    public class EventosPartidosConfiguration : IEntityTypeConfiguration<EventosPartidos>
    {
        public void Configure(EntityTypeBuilder<EventosPartidos> builder)
        {
            builder.HasKey(e => e.EventoPartidoId);
            builder.Property(e => e.PartidoId)
                   .IsRequired();
            builder.Property(e => e.EquipoId)
                   .IsRequired();
            builder.Property(e => e.JugadorId)
                   .IsRequired(false);
            builder.Property(e => e.Minuto)
                   .IsRequired();
            builder.Property(e => e.Tipo)
                   .HasMaxLength(50)
                   .IsRequired(false);
            builder.Property(e => e.Detalle)
                   .HasMaxLength(50)
                   .IsRequired(false);
            builder.Property(e => e.Comentario)
                   .HasMaxLength(255)
                   .IsRequired(false);
            builder.HasIndex(e => e.PartidoId);
            builder.HasIndex(e => new { e.PartidoId, e.Minuto });

            // Relación con Partido (obligatoria)
            builder.HasOne(e => e.Partido)
                   .WithMany(p => p.Eventos)
                   .HasForeignKey(e => e.PartidoId)
                   .OnDelete(DeleteBehavior.Cascade);
       
            // Relación con Equipo (obligatoria)
            builder.HasOne(e => e.Equipo)
                   .WithMany()
                   .HasForeignKey(e => e.EquipoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relación con Jugador (opcional)
            builder.HasOne(e => e.Jugador)
                   .WithMany()
                   .HasForeignKey(e => e.JugadorId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
