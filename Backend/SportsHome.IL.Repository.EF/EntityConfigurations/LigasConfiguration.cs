using SportsHome.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SportsHome.IL.Repository.EF.EntityConfigurations
{
    public class LigasConfiguration : IEntityTypeConfiguration<Ligas>
    {
        public void Configure(EntityTypeBuilder<Ligas> builder)
        {
            builder.HasKey(l => l.LigaId);
            builder.Property(l => l.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(l => l.Pais).IsRequired().HasMaxLength(100);
            builder.Property(l => l.Logo).HasMaxLength(255);
            builder.Property(l => l.Temporada).IsRequired();
        }
    }
}
