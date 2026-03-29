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
    public class PartidosController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPartidosService _partidoService;
        private readonly IMapper _mapper;

        public PartidosController(ILogger<PartidosController> logger, IMapper mapper, IPartidosService partidosService)
        {
            _logger = logger;
            _mapper = mapper;
            _partidoService = partidosService;
        }

        // GET /api/partidos
        [HttpGet("/api/partidos")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Partidos> partidos = await _partidoService.GetListAsync();
            IEnumerable<PartidosResource> partidosResources = _mapper.Map<IEnumerable<PartidosResource>>(partidos);
            return Ok(partidosResources);
        }

        // GET /api/partidos/{partidoId}
        [HttpGet("/api/partidos/{partidoId}")]
        public async Task<IActionResult> Get([FromRoute] int partidoId)
        {
            Partidos? partido = await _partidoService.GetAsync(partidoId);
            if (partido == null)
                return NotFound();

            PartidosResource partidosResource = _mapper.Map<PartidosResource>(partido);
            return Ok(partidosResource);
        }

        // POST /api/partidos
        [HttpPost("/api/partidos")]
        public async Task<IActionResult> Post([FromBody] PartidosResource partido)
        {
            if (partido == null)
                return BadRequest();

            Partidos? p = await _partidoService.AddAsync(_mapper.Map<Partidos>(partido));
            PartidosResource partidosResource = _mapper.Map<PartidosResource>(p);
            return CreatedAtAction(nameof(Get), new { partidoId = partidosResource.PartidoId }, partidosResource);
        }

        // PUT /api/partidos/{partidoId}
        [HttpPut("/api/partidos/{partidoId}")]
        public async Task<IActionResult> Put([FromRoute] int partidoId, [FromBody] PartidosResource partidoResource)
        {
            if (partidoResource == null)
                return BadRequest();


            Partidos? partidoOld = await _partidoService.GetAsync(partidoId);
            if (partidoOld == null)
                return NotFound();


            await _partidoService.Update(partidoOld, _mapper.Map<Partidos>(partidoResource));
            return NoContent();
        }

        // DELETE /api/partidos/{partidoId}
        [HttpDelete("/api/partidos/{partidoId}")]
        public async Task<IActionResult> Delete([FromRoute] int partidoId)
        {
            Partidos? partido = await _partidoService.GetAsync(partidoId);

            if (partido == null)
                return NotFound();
            try
            {
                await _partidoService.DeleteAsync(partido);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { Error = e.Message });
            }

        }
    }
}

