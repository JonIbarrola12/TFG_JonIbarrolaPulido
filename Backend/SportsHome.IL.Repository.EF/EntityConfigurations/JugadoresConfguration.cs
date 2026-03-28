using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHome.Core.Entities;

namespace SportsHome.IL.Repository.EF.EntityConfigurations
{
    public class JugadoresConfiguration : IEntityTypeConfiguration<Jugadores>
    {
        public void Configure(EntityTypeBuilder<Jugadores> builder)
        {
            builder.HasKey(j => j.JugadorId);
            builder.Property(j => j.ExternalId)
                   .IsRequired();
            builder.HasIndex(j => j.ExternalId)
                   .IsUnique();
            builder.Property(j => j.Nombre)
                   .IsRequired()
                   .HasMaxLength(150);
            builder.Property(j => j.NombrePropio)
                   .HasMaxLength(100)
                   .IsRequired(false);
            builder.Property(j => j.Apellido)
                   .HasMaxLength(100)
                   .IsRequired(false);
            builder.Property(j => j.Edad)
                   .IsRequired(false);
            builder.Property(j => j.Nacionalidad)
                   .HasMaxLength(100)
                   .IsRequired(false);
            builder.Property(j => j.Altura)
                   .HasMaxLength(10)
                   .IsRequired(false);
            builder.Property(j => j.Peso)
                   .HasMaxLength(10)
                   .IsRequired(false);
            builder.Property(j => j.Foto)
                   .HasMaxLength(255)
                   .IsRequired(false);
        }
    }
}
