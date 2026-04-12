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
    public class EquiposController : ControllerBase
    {
        private readonly IEquiposService _equipoService;
        private readonly SportsHomeContext _context;
        private readonly IMapper _mapper;

        public EquiposController(IMapper mapper, IEquiposService equiposService, SportsHomeContext context)
        {
            _mapper = mapper;
            _equipoService = equiposService;
            _context = context;
        }

        // GET /api/equipos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var equipos = await _equipoService.GetListAsync();
            return Ok(_mapper.Map<IEnumerable<EquiposResource>>(equipos));
        }

        // GET /api/equipos/5
        [HttpGet("{equipoId}")]
        public async Task<IActionResult> GetById(int equipoId)
        {
            var equipo = await _equipoService.GetAsync(equipoId);
            if (equipo == null) return NotFound();
            return Ok(_mapper.Map<EquiposResource>(equipo));
        }

        // GET /api/equipos/5/jugadores?temporada=2024
        [HttpGet("{equipoId}/jugadores")]
        public async Task<IActionResult> GetJugadores(int equipoId, [FromQuery] int? temporada)
        {
            var temp = temporada ?? DateTime.UtcNow.Year - 1;

            var equipo = await _context.Equipos.FindAsync(equipoId);
            if (equipo == null) return NotFound();

            var jugadores = await _context.JugadoresEquipos
                .Include(je => je.Jugador)
                .Where(je => je.EquipoId == equipoId && je.Temporada == temp)
                .Select(je => je.Jugador)
                .OrderBy(j => j.Nombre)
                .ToListAsync();

            return Ok(_mapper.Map<List<JugadoresResource>>(jugadores));
        }

        // GET /api/equipos/5/partidos?temporada=2024
        [HttpGet("{equipoId}/partidos")]
        public async Task<IActionResult> GetPartidos(int equipoId, [FromQuery] int? temporada)
        {
            var temp = temporada ?? DateTime.UtcNow.Year - 1;

            var equipo = await _context.Equipos.FindAsync(equipoId);
            if (equipo == null) return NotFound();

            var partidos = await _context.Partidos
                .Include(p => p.EquipoLocal)
                .Include(p => p.EquipoVisitante)
                .Include(p => p.Liga)
                .Where(p => (p.EquipoLocalId == equipoId || p.EquipoVisitanteId == equipoId)
                            && p.Temporada == temp)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return Ok(_mapper.Map<List<PartidosResource>>(partidos));
        }

        // GET /api/equipos/5/estadisticas?temporada=2024
        [HttpGet("{equipoId}/estadisticas")]
        public async Task<IActionResult> GetEstadisticas(int equipoId, [FromQuery] int? temporada)
        {
            var temp = temporada ?? DateTime.UtcNow.Year - 1;

            var stats = await _context.EstadisticasJugadores
                .Include(e => e.Jugador)
                .Include(e => e.Liga)
                .Where(e => e.EquipoId == equipoId && e.Temporada == temp)
                .OrderByDescending(e => e.Goles)
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(stats));
        }
    }
}
