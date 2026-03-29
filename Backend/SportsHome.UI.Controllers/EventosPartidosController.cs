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
    public class EventosPartidosController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEventosPartidosService _eventoPartidoService;
        private readonly IMapper _mapper;

        public EventosPartidosController(ILogger<EventosPartidosController> logger, IMapper mapper, IEventosPartidosService eventosPartidosService)
        {
            _logger = logger;
            _mapper = mapper;
            _eventoPartidoService = eventosPartidosService;
        }

        // GET /api/eventospartidos
        [HttpGet("/api/eventospartidos")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<EventosPartidos> eventosPartidos = await _eventoPartidoService.GetListAsync();
            IEnumerable<EventosPartidosResource> eventosPartidosResources = _mapper.Map<IEnumerable<EventosPartidosResource>>(eventosPartidos);
            return Ok(eventosPartidosResources);
        }

        // GET /api/eventospartidos/{eventoPartidoId}
        [HttpGet("/api/eventospartidos/{eventoPartidoId}")]
        public async Task<IActionResult> Get([FromRoute] int eventoPartidoId)
        {
            EventosPartidos? eventoPartido = await _eventoPartidoService.GetAsync(eventoPartidoId);
            if (eventoPartido == null)
                return NotFound();

            EventosPartidosResource eventoPartidoResource = _mapper.Map<EventosPartidosResource>(eventoPartido);
            return Ok(eventoPartidoResource);
        }

        // POST /api/eventospartidos
        [HttpPost("/api/eventospartidos")]
        public async Task<IActionResult> Post([FromBody] EventosPartidosResource eventoPartido)
        {
            if (eventoPartido == null)
                return BadRequest();

            EventosPartidos? e = await _eventoPartidoService.AddAsync(_mapper.Map<EventosPartidos>(eventoPartido));
            EventosPartidosResource eventoPartidoResource = _mapper.Map<EventosPartidosResource>(e);
            return CreatedAtAction(nameof(Get), new { eventoPartidoId = eventoPartidoResource.EventoPartidoId }, eventoPartidoResource);
        }

        // PUT /api/eventospartidos/{eventoPartidoId}
        [HttpPut("/api/eventospartidos/{eventoPartidoId}")]
        public async Task<IActionResult> Put([FromRoute] int eventoPartidoId, [FromBody] EventosPartidosResource eventoPartidoResource)
        {
            if (eventoPartidoResource == null)
                return BadRequest();


            EventosPartidos? eventoPartidoOld = await _eventoPartidoService.GetAsync(eventoPartidoId);  
            if (eventoPartidoOld == null)
                return NotFound();


            await _eventoPartidoService.Update(eventoPartidoOld, _mapper.Map<EventosPartidos>(eventoPartidoResource));
            return NoContent();
        }

        // DELETE /api/eventospartidos/{eventoPartidoId}
        [HttpDelete("/api/eventospartidos/{eventoPartidoId}")]
        public async Task<IActionResult> Delete([FromRoute] int eventoPartidoId)
        {
            EventosPartidos? eventoPartido = await _eventoPartidoService.GetAsync(eventoPartidoId);

            if (eventoPartido == null)
                return NotFound();
            try
            {
                await _eventoPartidoService.DeleteAsync(eventoPartido);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { Error = e.Message });
            }

        }
    }
}
