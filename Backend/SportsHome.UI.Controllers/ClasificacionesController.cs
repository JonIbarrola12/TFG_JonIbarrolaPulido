using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces.Services;
using SportsHome.UI.Controllers.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.UI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/[controller]")]
    [ApiController]
    public class ClasificacionesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IClasificacionesService _clasificacionService;
        private readonly IMapper _mapper;

        public ClasificacionesController(ILogger<EquiposController> logger, IMapper mapper, IClasificacionesService clasificacionesService)
        {
            _logger = logger;
            _mapper = mapper;
            _clasificacionService = clasificacionesService;
        }

        // GET /api/clasificaciones
        [HttpGet("/api/clasificaciones")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Clasificaciones> clasificaciones = await _clasificacionService.GetListAsync();
            IEnumerable<ClasificacionesResource> clasificacionesResources = _mapper.Map<IEnumerable<ClasificacionesResource>>(clasificaciones);
            return Ok(clasificacionesResources);
        }

        // GET /api/clasificaciones/{clasificacionId}
        [HttpGet("/api/clasificaciones/{clasificacionId}")]
        public async Task<IActionResult> Get([FromRoute] int clasificacionId)
        {
            Clasificaciones? clasificacion = await _clasificacionService.GetAsync(clasificacionId);
            if (clasificacion == null)
                return NotFound();

            ClasificacionesResource clasificacionResource = _mapper.Map<ClasificacionesResource>(clasificacion);
            return Ok(clasificacionResource);
        }

        // POST /api/clasificaciones
        [HttpPost("/api/clasificaciones")]
        public async Task<IActionResult> Post([FromBody] ClasificacionesResource clasificacion)
        {
            if (clasificacion == null)
                return BadRequest();

            Clasificaciones? c = await _clasificacionService.AddAsync(_mapper.Map<Clasificaciones>(clasificacion));
            ClasificacionesResource clasificacionResource = _mapper.Map<ClasificacionesResource>(c);
            return CreatedAtAction(nameof(Get), new { clasificacionId = clasificacionResource.ClasificacionId }, clasificacionResource);
        }

        // PUT /api/clasificaciones/{clasificacionId}
        [HttpPut("/api/clasificaciones/{clasificacionId}")]
        public async Task<IActionResult> Put([FromRoute] int clasificacionId, [FromBody] ClasificacionesResource clasificacionResource)
        {
            if (clasificacionResource == null)
                return BadRequest();


            Clasificaciones? clasificacionOld = await _clasificacionService.GetAsync(clasificacionId);  
            if (clasificacionOld == null)
                return NotFound();


            await _clasificacionService.Update(clasificacionOld, _mapper.Map<Clasificaciones>(clasificacionResource));
            return NoContent();
        }

        // DELETE /api/clasificaciones/{clasificacionId}
        [HttpDelete("/api/clasificaciones/{clasificacionId}")]
        public async Task<IActionResult> Delete([FromRoute] int clasificacionId)
        {
            Clasificaciones? clasificacion = await _clasificacionService.GetAsync(clasificacionId);

            if (clasificacion == null)
                return NotFound();
            try
            {
                await _clasificacionService.DeleteAsync(clasificacion);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }
    }
}
