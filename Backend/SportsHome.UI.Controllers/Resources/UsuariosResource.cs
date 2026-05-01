namespace SportsHome.UI.Controllers.Resources
{
    public class UsuariosResource
    {
        public int UsuarioId { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string NombreUsuario { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? UrlAvatar { get; set; }
        public string Rol { get; set; }
        public DateTime CreadoEn { get; set; }
    }
    public class RegistroUsuarioResource
    {
        public string Correo { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
    }
    public class LoginUsuarioResource
    {
        public string Correo { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public int UsuarioId { get; set; }
        public string? UrlAvatar { get; set; }
    }
}
