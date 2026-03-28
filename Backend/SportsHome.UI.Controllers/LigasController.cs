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
    public class LigasController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILigasService _ligaService;
        private readonly IMapper _mapper;

        public LigasController(ILogger<LigasController> logger, IMapper mapper, ILigasService ligasService)
        {
            _logger = logger;
            _mapper = mapper;
            _ligaService = ligasService;
        }

        // GET /api/ligas
        [HttpGet("/api/ligas")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Ligas> ligas = await _ligaService.GetListAsync();
            IEnumerable<LigasResource> ligasResources = _mapper.Map<IEnumerable<LigasResource>>(ligas);
            return Ok(ligasResources);
        }

        // GET /api/ligas/{/api/ligas/}
        [HttpGet("/api/ligas/{ligaId}")]
        public async Task<IActionResult> Get([FromRoute] int ligaId)
        {
            Ligas? liga = await _ligaService.GetAsync(ligaId);
            if (liga == null)
                return NotFound();

            LigasResource ligasResource = _mapper.Map<LigasResource>(liga);
            return Ok(ligasResource);
        }

        // POST /api/ligas
        [HttpPost("/api/ligas")]
        public async Task<IActionResult> Post([FromBody] LigasResource liga)
        {
            if (liga == null)
                return BadRequest();

            Ligas? l = await _ligaService.AddAsync(_mapper.Map<Ligas>(liga));
            LigasResource ligasResource = _mapper.Map<LigasResource>(l);
            return CreatedAtAction(nameof(Get), new { ligaId = ligasResource.LigaId }, ligasResource);
        }

        // PUT /api/ligas/{ligaId}
        [HttpPut("/api/ligas/{ligaId}")]
        public async Task<IActionResult> Put([FromRoute] int ligaId, [FromBody] LigasResource ligaResource)
        {
            if (ligaResource == null)
                return BadRequest();


            Ligas? ligaOld = await _ligaService.GetAsync(ligaId);

            if (ligaOld == null)
                return NotFound();


            await _ligaService.Update(ligaOld, _mapper.Map<Ligas>(ligaResource));
            return NoContent();
        }

        // DELETE /api/ligas/{ligaId}
        [HttpDelete("/api/ligas/{ligaId}")]
        public async Task<IActionResult> Delete([FromRoute] int ligaId)
        {
            Ligas? liga = await _ligaService.GetAsync(ligaId);

            if (liga == null)
                return NotFound();
            try
            {
                await _ligaService.DeleteAsync(liga);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { Error = e.Message });
            }

        }
    }
}
