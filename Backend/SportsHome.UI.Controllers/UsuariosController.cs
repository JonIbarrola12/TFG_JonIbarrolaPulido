using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces.Services;
using SportsHome.Core.Services;
using SportsHome.UI.Controllers.Resources;

namespace SportsHome.UI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;
        private readonly IMapper _mapper;
        private readonly PasswordService _passwordService;

        public UsuariosController(IMapper mapper, IUsuariosService usuariosService, PasswordService passwordService)
        {
            _mapper = mapper;
            _usuariosService = usuariosService;
            _passwordService = passwordService;
        }

        // GET /api/usuarios
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuariosService.GetListAsync();
            return Ok(_mapper.Map<IEnumerable<UsuariosResource>>(usuarios));
        }

        // GET /api/usuarios/5
        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> GetById(int usuarioId)
        {
            var usuario = await _usuariosService.GetAsync(usuarioId);
            if (usuario == null) return NotFound();
            return Ok(_mapper.Map<UsuariosResource>(usuario));
        }

        // POST /api/usuarios
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuariosResource resource)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = _mapper.Map<UsuariosResource, Usuarios>(resource);
            usuario.CreadoEn = DateTime.UtcNow;

            var result = await _usuariosService.AddAsync(usuario);
            var resultResource = _mapper.Map<Usuarios, UsuariosResource>(result!);

            return CreatedAtAction(nameof(GetById), new { usuarioId = resultResource.UsuarioId }, resultResource);
        }

        // PUT /api/usuarios/5
        [HttpPut("{usuarioId}")]
        public async Task<IActionResult> Update(int usuarioId, [FromBody] UsuariosResource resource)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _usuariosService.GetAsync(usuarioId);
            if (usuario == null) return NotFound();

            var usuarioNew = _mapper.Map<UsuariosResource, Usuarios>(resource);
            await _usuariosService.Update(usuario, usuarioNew);

            return Ok(_mapper.Map<Usuarios, UsuariosResource>(usuario));
        }

        // DELETE /api/usuarios/5
        [HttpDelete("{usuarioId}")]
        public async Task<IActionResult> Delete(int usuarioId)
        {
            var usuario = await _usuariosService.GetAsync(usuarioId);
            if (usuario == null) return NotFound();

            await _usuariosService.DeleteAsync(usuario);
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistroUsuarioResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new Usuarios
            {
                Correo = resource.Correo,
                NombreUsuario = resource.NombreUsuario,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(resource.Contrasena),
                CreadoEn = DateTime.UtcNow,
                Rol = "USUARIO"
            };

            var result = await _usuariosService.AddAsync(usuario);

            var response = _mapper.Map<UsuariosResource>(result);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest resource)
        {
            var usuario = await _usuariosService.GetByEmailAsync(resource.Correo);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            var passwordOk = BCrypt.Net.BCrypt.Verify(resource.Contrasena, usuario.Contrasena);

            if (!passwordOk)
                return Unauthorized("Contraseña incorrecta");

            return Ok(new LoginUsuarioResource
            {
                Correo = usuario.Correo,
                NombreUsuario = usuario.NombreUsuario,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                UsuarioId = usuario.UsuarioId,
                UrlAvatar = usuario.UrlAvatar
            });
        }

        [HttpPut("perfil/{usuarioId}")]
        public async Task<IActionResult> UpdatePerfil(int usuarioId, [FromBody] UpdatePerfilResource resource)
        {
            var usuario = await _usuariosService.GetAsync(usuarioId);

            if (usuario == null)
                return NotFound();

            usuario.Nombre = resource.Nombre ?? usuario.Nombre;
            usuario.Apellidos = resource.Apellidos ?? usuario.Apellidos;
            usuario.NombreUsuario = resource.NombreUsuario ?? usuario.NombreUsuario;
            usuario.UrlAvatar = resource.UrlAvatar ?? usuario.UrlAvatar;

            await _usuariosService.Update(usuario, usuario); 

            return Ok(_mapper.Map<UsuariosResource>(usuario));
        }

        [HttpPost("upload-avatar/{usuarioId}")]
        public async Task<IActionResult> UploadAvatar(int usuarioId, IFormFile file)
        {
            var usuario = await _usuariosService.GetAsync(usuarioId);

            if (usuario == null)
                return NotFound();

            if (file == null || file.Length == 0)
                return BadRequest("Archivo inválido");

            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("Solo se permiten imágenes");

            // BORRAR AVATAR ANTERIOR
            if (!string.IsNullOrEmpty(usuario.UrlAvatar))
            {
                var oldFileName = Path.GetFileName(usuario.UrlAvatar);
                var oldPath = Path.Combine("wwwroot/avatars", oldFileName);

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            // GUARDAR NUEVA IMAGEN
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine("wwwroot/avatars", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"https://localhost:7085/avatars/{fileName}";

            usuario.UrlAvatar = url;
            await _usuariosService.UpdateAsync(usuario);

            return Ok(new { url });
        }
    }
}
