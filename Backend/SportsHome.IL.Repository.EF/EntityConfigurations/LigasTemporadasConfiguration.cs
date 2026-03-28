using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHome.Core.Entities;


namespace SportsHome.IL.Repository.EF.EntityConfigurations
{
    public class LigasTemporadasConfiguration : IEntityTypeConfiguration<LigasTemporadas>
    {
        public void Configure(EntityTypeBuilder<LigasTemporadas> builder)
        {
            builder.HasKey(lt => lt.LigaTemporadaId);
            builder.Property(lt => lt.LigaId)
                   .IsRequired();
            builder.Property(lt => lt.Temporada)
                   .IsRequired();
            builder.HasIndex(lt => new { lt.LigaId, lt.Temporada })
                   .IsUnique();

            //Relación con Liga
            builder.HasOne(lt => lt.Liga)
                   .WithMany(l => l.Temporadas)
                   .HasForeignKey(lt => lt.LigaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
