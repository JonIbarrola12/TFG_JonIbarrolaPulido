using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsHome.Core.Entities;

namespace SportsHome.IL.Repository.EF.EntityConfigurations
{
    public class UsuariosConfiguration : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.UsuarioId);

            builder.Property(u => u.UsuarioId)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Correo)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Correo)
                .IsUnique();

            builder.Property(u => u.NombreUsuario)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.NombreUsuario)
                .IsUnique();

            builder.Property(u => u.Contrasena)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Nombre)
                .HasMaxLength(100);

            builder.Property(u => u.Apellidos)
                .HasMaxLength(150);

            builder.Property(u => u.UrlAvatar)
                .HasMaxLength(500);

            builder.Property(u => u.Rol)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("USUARIO");

            builder.Property(u => u.CreadoEn)
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}