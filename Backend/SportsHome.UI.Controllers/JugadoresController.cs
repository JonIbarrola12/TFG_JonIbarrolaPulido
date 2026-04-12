using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsHome.Core.Interfaces.Services;
using SportsHome.IL.Repository.EF;
using SportsHome.UI.Controllers.Resources;

namespace SportsHome.UI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/[controller]")]
    [ApiController]
    public class JugadoresController : ControllerBase
    {
        private readonly IJugadoresService _jugadorService;
        private readonly SportsHomeContext _context;
        private readonly IMapper _mapper;

        public JugadoresController(IMapper mapper, IJugadoresService jugadoresService, SportsHomeContext context)
        {
            _mapper = mapper;
            _jugadorService = jugadoresService;
            _context = context;
        }

        // GET /api/jugadores/5
        [HttpGet("{jugadorId}")]
        public async Task<IActionResult> GetById(int jugadorId)
        {
            var jugador = await _jugadorService.GetAsync(jugadorId);
            if (jugador == null) return NotFound();
            return Ok(_mapper.Map<JugadoresResource>(jugador));
        }

        // GET /api/jugadores/5/estadisticas
        [HttpGet("{jugadorId}/estadisticas")]
        public async Task<IActionResult> GetEstadisticas(int jugadorId)
        {
            var jugador = await _context.Jugadores.FindAsync(jugadorId);
            if (jugador == null) return NotFound();

            var stats = await _context.EstadisticasJugadores
                .Include(e => e.Equipo)
                .Include(e => e.Liga)
                .Include(e => e.Jugador)
                .Where(e => e.JugadorId == jugadorId)
                .OrderByDescending(e => e.Temporada)
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(stats));
        }

        // GET /api/jugadores/buscar?nombre=messi
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("El parametro 'nombre' es obligatorio.");

            var jugadores = await _context.Jugadores
                .Where(j => j.Nombre.Contains(nombre) ||
                            j.NombrePropio.Contains(nombre) ||
                            j.Apellido.Contains(nombre))
                .OrderBy(j => j.Nombre)
                .Take(50)
                .ToListAsync();

            return Ok(_mapper.Map<List<JugadoresResource>>(jugadores));
        }
    }
}
