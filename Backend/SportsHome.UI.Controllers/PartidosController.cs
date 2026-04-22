using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsHome.IL.Repository.EF;
using SportsHome.UI.Controllers.Resources;

namespace SportsHome.UI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/[controller]")]
    [ApiController]
    public class PartidosController : ControllerBase
    {
        private readonly SportsHomeContext _context;
        private readonly IMapper _mapper;

        public PartidosController(IMapper mapper, SportsHomeContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET /api/partidos/5
        [HttpGet("{partidoId}")]
        public async Task<IActionResult> GetById(int partidoId)
        {
            var partido = await _context.Partidos
                .Include(p => p.EquipoLocal)
                .Include(p => p.EquipoVisitante)
                .Include(p => p.Liga)
                .FirstOrDefaultAsync(p => p.PartidoId == partidoId);

            if (partido == null) return NotFound();

            return Ok(_mapper.Map<PartidosResource>(partido));
        }

        // GET /api/partidos/jornada?ligaId=5&temporada=2024&ronda=Regular Season - 10
        [HttpGet("jornada")]
        public async Task<IActionResult> GetByJornada([FromQuery] int ligaId, [FromQuery] int? temporada, [FromQuery] string ronda)
        {
            var temp = temporada ?? 2024;

            var query = _context.Partidos
                .Include(p => p.EquipoLocal)
                .Include(p => p.EquipoVisitante)
                .Where(p => p.LigaId == ligaId && p.Temporada == temp);

            if (!string.IsNullOrWhiteSpace(ronda))
                query = query.Where(p => p.Ronda == ronda);

            var partidos = await query
                .OrderBy(p => p.Fecha)
                .ToListAsync();

            return Ok(_mapper.Map<List<PartidosResource>>(partidos));
        }

        // GET /api/partidos/proximos?ligaId=5&cantidad=10
        [HttpGet("proximos")]
        public async Task<IActionResult> GetProximos([FromQuery] int? ligaId, [FromQuery] int cantidad = 10)
        {
            var ahora = DateTime.UtcNow;

            var query = _context.Partidos
                .Include(p => p.EquipoLocal)
                .Include(p => p.EquipoVisitante)
                .Include(p => p.Liga)
                .Where(p => p.Fecha > ahora);

            if (ligaId.HasValue)
                query = query.Where(p => p.LigaId == ligaId.Value);

            var partidos = await query
                .OrderBy(p => p.Fecha)
                .Take(cantidad)
                .ToListAsync();

            return Ok(_mapper.Map<List<PartidosResource>>(partidos));
        }

        // GET /api/partidos/ultimos?ligaId=5&cantidad=10
        [HttpGet("ultimos")]
        public async Task<IActionResult> GetUltimos([FromQuery] int? ligaId, [FromQuery] int cantidad = 10)
        {
            var ahora = DateTime.UtcNow;

            var query = _context.Partidos
                .Include(p => p.EquipoLocal)
                .Include(p => p.EquipoVisitante)
                .Include(p => p.Liga)
                .Where(p => p.Fecha <= ahora && p.Estado == "FT");

            if (ligaId.HasValue)
                query = query.Where(p => p.LigaId == ligaId.Value);

            var partidos = await query
                .OrderByDescending(p => p.Fecha)
                .Take(cantidad)
                .ToListAsync();

            return Ok(_mapper.Map<List<PartidosResource>>(partidos));
        }
    }
}
