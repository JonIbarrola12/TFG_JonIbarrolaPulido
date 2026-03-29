using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces.Services;
using SportsHome.UI.Controllers.Resources;

namespace SportsHome.UI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticasJugadoresController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEstadisticasJugadoresService _estadisticaJugadorService;
        private readonly IMapper _mapper;

        public EstadisticasJugadoresController(ILogger<EquiposController> logger, IMapper mapper, IEstadisticasJugadoresService estadisticasJugadoresService)
        {
            _logger = logger;
            _mapper = mapper;
            _estadisticaJugadorService = estadisticasJugadoresService;
        }

        // GET /api/estadisticasjugadores
        [HttpGet("/api/estadisticasjugadores")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<EstadisticasJugadores> estadisticas = await _estadisticaJugadorService.GetListAsync();
            IEnumerable<EstadisticasJugadoresResource> estadisticasResources = _mapper.Map<IEnumerable<EstadisticasJugadoresResource>>(estadisticas);
            return Ok(estadisticasResources);
        }

        // GET /api/estadisticasjugadores/{estadisticaJugadorId}
        [HttpGet("/api/estadisticasjugadores/{estadisticaJugadorId}")]
        public async Task<IActionResult> Get([FromRoute] int estadisticaJugadorId)
        {
            EstadisticasJugadores? estadisticaJugador = await _estadisticaJugadorService.GetAsync(estadisticaJugadorId);
            if (estadisticaJugador == null)
                return NotFound();

            EstadisticasJugadoresResource estadisticaJugadorResource = _mapper.Map<EstadisticasJugadoresResource>(estadisticaJugador);
            return Ok(estadisticaJugadorResource);
        }

        // POST /api/estadisticasjugadores
        [HttpPost("/api/estadisticasjugadores")]
        public async Task<IActionResult> Post([FromBody] EstadisticasJugadoresResource estadisticaJugador)
        {
            if (estadisticaJugador == null)
                return BadRequest();

            EstadisticasJugadores? e = await _estadisticaJugadorService.AddAsync(_mapper.Map<EstadisticasJugadores>(estadisticaJugador));
            EstadisticasJugadoresResource estadisticaJugadorResource = _mapper.Map<EstadisticasJugadoresResource>(e);
            return CreatedAtAction(nameof(Get), new { estadisticaJugadorId = estadisticaJugadorResource.EstadisticaJugadorId }, estadisticaJugadorResource);
        }

        // PUT /api/estadisticasjugadores/{estadisticaJugadorId}
        [HttpPut("/api/estadisticasjugadores/{estadisticaJugadorId}")]
        public async Task<IActionResult> Put([FromRoute] int estadisticaJugadorId, [FromBody] EstadisticasJugadoresResource estadisticaJugadorResource  )
        {
            if (estadisticaJugadorResource == null)
                return BadRequest();


            EstadisticasJugadores? estadisticaJugadorOld = await _estadisticaJugadorService.GetAsync(estadisticaJugadorId); 
            if (estadisticaJugadorOld == null)
                return NotFound();


            await _estadisticaJugadorService.Update(estadisticaJugadorOld, _mapper.Map<EstadisticasJugadores>(estadisticaJugadorResource));
            return NoContent();
        }

        // DELETE /api/estadisticasjugadores/{estadisticaJugadorId}
        [HttpDelete("/api/estadisticasjugadores/{estadisticaJugadorId}")]
        public async Task<IActionResult> Delete([FromRoute] int estadisticaJugadorId)
        {
            EstadisticasJugadores? estadisticaJugador = await _estadisticaJugadorService.GetAsync(estadisticaJugadorId);

            if (estadisticaJugador == null)
                return NotFound();
            try
            {
                await _estadisticaJugadorService.DeleteAsync(estadisticaJugador);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { Error = e.Message });
            }

        }
    }
}
