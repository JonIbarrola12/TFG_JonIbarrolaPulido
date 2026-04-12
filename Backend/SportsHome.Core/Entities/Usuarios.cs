
namespace SportsHome.Core.Entities
{
    public class Usuarios
    {
        public int UsuarioId { get; set; }
        public string Correo { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? UrlAvatar { get; set; }
        public string Rol { get; set; } = "USUARIO";
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}
