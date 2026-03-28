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
    public class EquiposController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEquiposService _equipoService;
        private readonly IMapper _mapper;

        public EquiposController(ILogger<EquiposController> logger, IMapper mapper, IEquiposService equiposService)
        {
            _logger = logger;
            _mapper = mapper;
            _equipoService = equiposService;
        }

        // GET /api/equipos
        [HttpGet("/api/equipos")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Equipos> equipos = await _equipoService.GetListAsync();
            IEnumerable<EquiposResource> equiposResources = _mapper.Map<IEnumerable<EquiposResource>>(equipos);
            return Ok(equiposResources);
        }

        // GET /api/equipos/{/api/equipos/}
        [HttpGet("/api/equipos/{equipoId}")]
        public async Task<IActionResult> Get([FromRoute] int equipoId)
        {
            Equipos? equipo = await _equipoService.GetAsync(equipoId);
            if (equipo == null)
                return NotFound();

            EquiposResource equiposResource = _mapper.Map<EquiposResource>(equipo);
            return Ok(equiposResource);
        }

        // POST /api/equipos
        [HttpPost("/api/equipos")]
        public async Task<IActionResult> Post([FromBody] EquiposResource equipo)
        {
            if (equipo == null)
                return BadRequest();

            Equipos? e = await _equipoService.AddAsync(_mapper.Map<Equipos>(equipo));
            EquiposResource equiposResource = _mapper.Map<EquiposResource>(e);
            return CreatedAtAction(nameof(Get), new { equipoId = equiposResource.EquipoId }, equiposResource);
        }

        // PUT /api/equipos/{equipoId}
        [HttpPut("/api/equipos/{equipoId}")]
        public async Task<IActionResult> Put([FromRoute] int equipoId, [FromBody] EquiposResource equipoResource)
        {
            if (equipoResource == null)
                return BadRequest();


            Equipos? equipoOld = await _equipoService.GetAsync(equipoId);

            if (equipoOld == null)
                return NotFound();


            await _equipoService.Update(equipoOld, _mapper.Map<Equipos>(equipoResource));
            return NoContent();
        }

        // DELETE /api/equipos/{equipoId}
        [HttpDelete("/api/equipos/{equipoId}")]
        public async Task<IActionResult> Delete([FromRoute] int equipoId)
        {
            Equipos? equipo = await _equipoService.GetAsync(equipoId);

            if (equipo == null)
                return NotFound();
            try
            {
                await _equipoService.DeleteAsync(equipo);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { Error = e.Message });
            }

        }
    }
}
