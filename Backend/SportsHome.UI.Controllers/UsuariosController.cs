using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces.Services;
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

        public UsuariosController(IMapper mapper, IUsuariosService usuariosService)
        {
            _mapper = mapper;
            _usuariosService = usuariosService;
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
    }
}
