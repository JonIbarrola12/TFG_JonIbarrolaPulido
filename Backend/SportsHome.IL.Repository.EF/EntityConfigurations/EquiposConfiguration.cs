using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHome.Core.Entities;

namespace SportsHome.IL.Repository.EF.EntityConfigurations
{
    public class EquiposConfiguration : IEntityTypeConfiguration<Equipos>
    {
        public void Configure(EntityTypeBuilder<Equipos> builder)
        {
            builder.HasKey(l => l.EquipoId);
            builder.Property(l => l.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(l => l.Pais).IsRequired().HasMaxLength(100);
            builder.Property(l => l.Logo).HasMaxLength(255);
            builder.Property(l => l.NombreEstadio).IsRequired().HasMaxLength(100);
            builder.Property(l => l.CapacidadEstadio).IsRequired(false);
        }
    }
}
