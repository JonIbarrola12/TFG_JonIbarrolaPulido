using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces.Services;
using SportsHome.UI.Controllers.Resources;

namespace SportsHome.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncLogsController : ControllerBase
    {
        private readonly ILogger<SyncLogsController> _logger;
        private readonly ISyncLogsService _syncService;
        private readonly IMapper _mapper;

        public SyncLogsController(
            ILogger<SyncLogsController> logger,
            IMapper mapper,
            ISyncLogsService syncService)
        {
            _logger = logger;
            _mapper = mapper;
            _syncService = syncService;
        }

        // GET /api/synclogs
        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _syncService.GetListAsync();
            return Ok(_mapper.Map<IEnumerable<SyncLogsResource>>(logs));
        }

        // POST /api/synclogs/full
        [HttpPost("full")]
        public async Task<IActionResult> SyncFull()
        {
            try
            {
                await _syncService.SyncFullAsync();
                return Ok("Sincronizacion completa ejecutada con exito.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sincronizar datos desde la API");
                return StatusCode(500, "Error al sincronizar datos: " + ex.Message);
            }
        }
    }
}
