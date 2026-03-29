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
    public class SyncLogsConfiguration : IEntityTypeConfiguration<SyncLogs>
    {
        public void Configure(EntityTypeBuilder<SyncLogs> builder)
        {
            builder.HasKey(s => s.SyncLogId);
            builder.Property(s => s.Entidad)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(s => s.UltimaSincronizacion)
                   .IsRequired();

            builder.HasIndex(s => s.Entidad)
                   .IsUnique();
        }
    }
}
