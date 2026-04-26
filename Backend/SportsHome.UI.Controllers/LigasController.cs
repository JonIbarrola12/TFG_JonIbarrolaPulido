using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces.Services;
using SportsHome.IL.Repository.EF;
using SportsHome.UI.Controllers.Resources;

namespace SportsHome.UI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/[controller]")]
    [ApiController]
    public class LigasController : ControllerBase
    {
        private readonly ILigasService _ligaService;
        private readonly SportsHomeContext _context;
        private readonly IMapper _mapper;

        public LigasController(IMapper mapper, ILigasService ligasService, SportsHomeContext context)
        {
            _mapper = mapper;
            _ligaService = ligasService;
            _context = context;
        }

        // GET /api/ligas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ligas = await _ligaService.GetListAsync();
            return Ok(_mapper.Map<IEnumerable<LigasResource>>(ligas));
        }

        // GET /api/ligas/5
        [HttpGet("{ligaId}")]
        public async Task<IActionResult> GetById(int ligaId)
        {
            var liga = await _ligaService.GetAsync(ligaId);
            if (liga == null) return NotFound();
            return Ok(_mapper.Map<LigasResource>(liga));
        }

        // GET /api/ligas/5/temporadas
        [HttpGet("{ligaId}/temporadas")]
        public async Task<IActionResult> GetTemporadas(int ligaId)
        {
            var liga = await _context.Ligas.FirstOrDefaultAsync(l => l.LigaId == ligaId);
            if (liga == null) return NotFound();

            var temporadas = await _context.Set<LigasTemporadas>()
                .Where(lt => lt.LigaId == ligaId)
                .OrderByDescending(lt => lt.Temporada)
                .ToListAsync();

            return Ok(_mapper.Map<List<LigasTemporadasResource>>(temporadas));
        }

        // GET /api/ligas/5/equipos?temporada=2024
        [HttpGet("{ligaId}/equipos")]
        public async Task<IActionResult> GetEquipos(int ligaId, [FromQuery] int? temporada)
        {
            var temp = temporada ?? 2024;

            var liga = await _context.Ligas.FirstOrDefaultAsync(l => l.LigaId == ligaId);
            if (liga == null) return NotFound();

            // Intentar primero por Clasificaciones
            var equipoIds = await _context.Clasificaciones
                .Where(c => c.LigaId == ligaId && c.Temporada == temp)
                .Select(c => c.EquipoId)
                .Distinct()
                .ToListAsync();

            // Si no hay clasificaciones, buscar por EstadisticasJugadores
            if (equipoIds.Count == 0)
            {
                equipoIds = await _context.EstadisticasJugadores
                    .Where(e => e.LigaId == ligaId && e.Temporada == temp)
                    .Select(e => e.EquipoId)
                    .Distinct()
                    .ToListAsync();
            }

            var equipos = await _context.Equipos
                .Where(e => equipoIds.Contains(e.EquipoId))
                .OrderBy(e => e.Nombre)
                .ToListAsync();

            return Ok(_mapper.Map<List<EquiposResource>>(equipos));
        }

        // GET /api/ligas/5/clasificacion?temporada=2024
        [HttpGet("{ligaId}/clasificacion")]
        public async Task<IActionResult> GetClasificacion(int ligaId, [FromQuery] int? temporada)
        {
            var temp = temporada ?? 2024;

            var clasificacion = await _context.Clasificaciones
                .Include(c => c.Equipo)
                .Include(c => c.Liga)
                .Where(c => c.LigaId == ligaId && c.Temporada == temp)
                .OrderBy(c => c.Posicion)
                .ToListAsync();

            if (clasificacion.Count == 0) return NotFound();

            return Ok(_mapper.Map<List<ClasificacionesResource>>(clasificacion));
        }

        // GET /api/ligas/5/partidos?temporada=2024
        [HttpGet("{ligaId}/partidos")]
        public async Task<IActionResult> GetPartidos(int ligaId, [FromQuery] int? temporada)
        {
            var temp = temporada ?? 2024;

            var partidos = await _context.Partidos
                .Include(p => p.EquipoLocal)
                .Include(p => p.EquipoVisitante)
                .Include(p => p.Liga)
                .Where(p => p.LigaId == ligaId && p.Temporada == temp)
                .OrderBy(p => p.Fecha)
                .ToListAsync();

            return Ok(_mapper.Map<List<PartidosResource>>(partidos));
        }

        // GET /api/ligas/5/goleadores?temporada=2024&top=20
        [HttpGet("{ligaId}/goleadores")]
        public async Task<IActionResult> GetGoleadores(int ligaId, [FromQuery] int? temporada, [FromQuery] int top = 20)
        {
            var temp = temporada ?? 2024;

            var goleadores = await _context.EstadisticasJugadores
                .Include(e => e.Jugador)
                .Include(e => e.Equipo)
                .Include(e => e.Liga)
                .Where(e => e.LigaId == ligaId && e.Temporada == temp && e.Goles > 0)
                .OrderByDescending(e => e.Goles)
                .Take(top)
                .ToListAsync();

            return Ok(_mapper.Map<List<EstadisticasJugadoresResource>>(goleadores));
        }
    }
}
