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
    public class JugadoresController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IJugadoresService _jugadorService;
        private readonly IMapper _mapper;

        public JugadoresController(ILogger<JugadoresController> logger, IMapper mapper, IJugadoresService jugadoresService)
        {
            _logger = logger;
            _mapper = mapper;
            _jugadorService = jugadoresService;
        }

        // GET /api/jugadores
        [HttpGet("/api/jugadores")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Jugadores> jugadores = await _jugadorService.GetListAsync();
            IEnumerable<JugadoresResource> jugadoresResources = _mapper.Map<IEnumerable<JugadoresResource>>(jugadores);
            return Ok(jugadoresResources);
        }

        // GET /api/jugadores/{jugadorId}
        [HttpGet("/api/jugadores/{jugadorId}")]
        public async Task<IActionResult> Get([FromRoute] int jugadorId)
        {
            Jugadores? jugador = await _jugadorService.GetAsync(jugadorId);
            if (jugador == null)
                return NotFound();

            JugadoresResource jugadoresResource = _mapper.Map<JugadoresResource>(jugador);
            return Ok(jugadoresResource);
        }

        // POST /api/jugadores
        [HttpPost("/api/jugadores")]
        public async Task<IActionResult> Post([FromBody] JugadoresResource jugador)
        {
            if (jugador == null)
                return BadRequest();

            Jugadores? j = await _jugadorService.AddAsync(_mapper.Map<Jugadores>(jugador));
            JugadoresResource jugadoresResource = _mapper.Map<JugadoresResource>(j);
            return CreatedAtAction(nameof(Get), new { jugadorId = jugadoresResource.JugadorId }, jugadoresResource);
        }

        // PUT /api/jugadores/{jugadorId}
        [HttpPut("/api/jugadores/{jugadorId}")]
        public async Task<IActionResult> Put([FromRoute] int jugadorId, [FromBody] JugadoresResource jugadorResource)
        {
            if (jugadorResource == null)
                return BadRequest();


            Jugadores? jugadorOld = await _jugadorService.GetAsync(jugadorId);
            if (jugadorOld == null)
                return NotFound();


            await _jugadorService.Update(jugadorOld, _mapper.Map<Jugadores>(jugadorResource));
            return NoContent();
        }

        // DELETE /api/jugadores/{jugadorId}
        [HttpDelete("/api/jugadores/{jugadorId}")]
        public async Task<IActionResult> Delete([FromRoute] int jugadorId)
        {
            Jugadores? jugador = await _jugadorService.GetAsync(jugadorId);

            if (jugador == null)
                return NotFound();
            try
            {
                await _jugadorService.DeleteAsync(jugador);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { Error = e.Message });
            }

        }
    }
}
