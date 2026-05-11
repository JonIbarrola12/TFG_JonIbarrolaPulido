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
        private readonly IEstadisticasJugadoresService _estadisticasService;
        private readonly SportsHomeContext _context;
        private readonly IMapper _mapper;

        public JugadoresController(IMapper mapper, IJugadoresService jugadoresService, IEstadisticasJugadoresService estadisticasService, SportsHomeContext context)
        {
            _mapper = mapper;
            _jugadorService = jugadoresService;
            _estadisticasService = estadisticasService;
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
                .Where(e => e.JugadorId == jugadorId && e.Temporada == 2024)
                .OrderByDescending(e => e.Temporada)
                .ToListAsync();

            if (!stats.Any()) return NotFound("No hay estadísticas disponibles para este jugador en la temporada 2024.");

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

        // GET /api/jugadores/estadisticas/ligas/5/top-goleadores
        [HttpGet("estadisticas/ligas/{ligaId}/top-goleadores")]
        public async Task<IActionResult> GetTop10Goleadores(int ligaId, [FromQuery] int? temporada = null)
        {
            var stats = await _estadisticasService.GetTop10GoleadoresAsync(ligaId, temporada);
            if (!stats.Any()) return NotFound("No hay estadísticas disponibles para esta liga.");

            var estadisticas = await _context.EstadisticasJugadores
                .Include(e => e.Jugador)
                .Include(e => e.Equipo)
                .Include(e => e.Liga)
                .Where(e => stats.Select(s => s.EstadisticaJugadorId).Contains(e.EstadisticaJugadorId))
                .OrderByDescending(e => e.Goles)
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(estadisticas));
        }

        // GET /api/jugadores/estadisticas/ligas/5/top-asistentes
        [HttpGet("estadisticas/ligas/{ligaId}/top-asistentes")]
        public async Task<IActionResult> GetTop10Asistentes(int ligaId, [FromQuery] int? temporada = null)
        {
            var stats = await _estadisticasService.GetTop10AsistentesAsync(ligaId, temporada);
            if (!stats.Any()) return NotFound("No hay estadísticas disponibles para esta liga.");

            var estadisticas = await _context.EstadisticasJugadores
                .Include(e => e.Jugador)
                .Include(e => e.Equipo)
                .Include(e => e.Liga)
                .Where(e => stats.Select(s => s.EstadisticaJugadorId).Contains(e.EstadisticaJugadorId))
                .OrderByDescending(e => e.Asistencias)
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(estadisticas));
        }

        // GET /api/jugadores/estadisticas/ligas/5/top-tarjetas-amarillas
        [HttpGet("estadisticas/ligas/{ligaId}/top-tarjetas-amarillas")]
        public async Task<IActionResult> GetTop10TarjetasAmarillas(int ligaId, [FromQuery] int? temporada = null)
        {
            var stats = await _estadisticasService.GetTop10TarjetasAmarillasAsync(ligaId, temporada);
            if (!stats.Any()) return NotFound("No hay estadísticas disponibles para esta liga.");

            var estadisticas = await _context.EstadisticasJugadores
                .Include(e => e.Jugador)
                .Include(e => e.Equipo)
                .Include(e => e.Liga)
                .Where(e => stats.Select(s => s.EstadisticaJugadorId).Contains(e.EstadisticaJugadorId))
                .OrderByDescending(e => e.TarjetasAmarillas)
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(estadisticas));
        }

        // GET /api/jugadores/estadisticas/ligas/5/top-tarjetas-rojas
        [HttpGet("estadisticas/ligas/{ligaId}/top-tarjetas-rojas")]
        public async Task<IActionResult> GetTop10TarjetasRojas(int ligaId, [FromQuery] int? temporada = null)
        {
            var stats = await _estadisticasService.GetTop10TarjetasRojasAsync(ligaId, temporada);
            if (!stats.Any()) return NotFound("No hay estadísticas disponibles para esta liga.");

            var estadisticas = await _context.EstadisticasJugadores
                .Include(e => e.Jugador)
                .Include(e => e.Equipo)
                .Include(e => e.Liga)
                .Where(e => stats.Select(s => s.EstadisticaJugadorId).Contains(e.EstadisticaJugadorId))
                .OrderByDescending(e => e.TarjetasRojas)
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(estadisticas));
        }

        // GET /api/jugadores/estadisticas/ligas/5/top-minutos
        [HttpGet("estadisticas/ligas/{ligaId}/top-minutos")]
        public async Task<IActionResult> GetTop10Minutos(int ligaId, [FromQuery] int? temporada = null)
        {
            var stats = await _estadisticasService.GetTop10MinutosAsync(ligaId, temporada);
            if (!stats.Any()) return NotFound("No hay estadísticas disponibles para esta liga.");

            var estadisticas = await _context.EstadisticasJugadores
                .Include(e => e.Jugador)
                .Include(e => e.Equipo)
                .Include(e => e.Liga)
                .Where(e => stats.Select(s => s.EstadisticaJugadorId).Contains(e.EstadisticaJugadorId))
                .OrderByDescending(e => e.Minutos)
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(estadisticas));
        }

        // GET /api/jugadores/estadisticas/top-mejores-jugadores
        [HttpGet("estadisticas/top-mejores-jugadores")]
        public async Task<IActionResult> GetTop10MejoresJugadores([FromQuery] int? temporada = null)
        {
            var stats = await _estadisticasService.GetTop10MejoresJugadoresAsync(temporada);
            if (!stats.Any()) return NotFound("No hay estadísticas disponibles.");

            var estadisticas = await _context.EstadisticasJugadores
                .Include(e => e.Jugador)
                .Include(e => e.Equipo)
                .Include(e => e.Liga)
                .Where(e => stats.Select(s => s.EstadisticaJugadorId).Contains(e.EstadisticaJugadorId))
                .OrderByDescending(e => (e.Goles ?? 0) + (e.Asistencias ?? 0))
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(estadisticas));
        }

        // GET /api/jugadores/estadisticas/top-mas-problematicos
        [HttpGet("estadisticas/top-mas-problematicos")]
        public async Task<IActionResult> GetTop10MasProblematicos([FromQuery] int? temporada = null)
        {
            var stats = await _estadisticasService.GetTop10MasProblematicosAsync(temporada);
            if (!stats.Any()) return NotFound("No hay estadísticas disponibles.");

            var estadisticas = await _context.EstadisticasJugadores
                .Include(e => e.Jugador)
                .Include(e => e.Equipo)
                .Include(e => e.Liga)
                .Where(e => stats.Select(s => s.EstadisticaJugadorId).Contains(e.EstadisticaJugadorId))
                .OrderByDescending(e => (e.TarjetasAmarillas ?? 0) + (e.TarjetasRojas ?? 0))
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(estadisticas));
        }

        // GET /api/jugadores/estadisticas/top-minutos-totales
        [HttpGet("estadisticas/top-minutos-totales")]
        public async Task<IActionResult> GetTop10MinutosTotales([FromQuery] int? temporada = null)
        {
            var stats = await _estadisticasService.GetTop10MinutosTotalesAsync(temporada);
            if (!stats.Any()) return NotFound("No hay estadísticas disponibles.");

            var estadisticas = await _context.EstadisticasJugadores
                .Include(e => e.Jugador)
                .Include(e => e.Equipo)
                .Include(e => e.Liga)
                .Where(e => stats.Select(s => s.EstadisticaJugadorId).Contains(e.EstadisticaJugadorId))
                .OrderByDescending(e => e.Minutos)
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(estadisticas));
        }
    }
}
