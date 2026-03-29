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
    public class SyncLogsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISyncLogsService _syncLogService;
        private readonly IMapper _mapper;

        public SyncLogsController(ILogger<SyncLogsController> logger, IMapper mapper, ISyncLogsService syncLogService)
        {
            _logger = logger;
            _mapper = mapper;
            _syncLogService = syncLogService;
        }

        // GET /api/synclogs
        [HttpGet("/api/synclogs")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<SyncLogs> syncLogs = await _syncLogService.GetListAsync();
            IEnumerable<SyncLogsResource> syncLogsResources = _mapper.Map<IEnumerable<SyncLogsResource>>(syncLogs);
            return Ok(syncLogsResources);
        }

        // GET /api/synclogs/{syncLogId}
        [HttpGet("/api/synclogs/{syncLogId}")]
        public async Task<IActionResult> Get([FromRoute] int syncLogId)
        {
            SyncLogs? syncLog = await _syncLogService.GetAsync(syncLogId);
            if (syncLog == null)
                return NotFound();

            SyncLogsResource syncLogsResource = _mapper.Map<SyncLogsResource>(syncLog);
            return Ok(syncLogsResource);
        }

        // POST /api/synclogs
        [HttpPost("/api/synclogs")]
        public async Task<IActionResult> Post([FromBody] SyncLogsResource syncLogResource)
        {
            if (syncLogResource == null)
                return BadRequest();

            SyncLogs? syncLog = await _syncLogService.AddAsync(_mapper.Map<SyncLogs>(syncLogResource));
            SyncLogsResource syncLogsResource = _mapper.Map<SyncLogsResource>(syncLog);
            return CreatedAtAction(nameof(Get), new { syncLogId = syncLogsResource.SyncLogId }, syncLogsResource);
        }

        // PUT /api/synclogs/{syncLogId}
        [HttpPut("/api/synclogs/{syncLogId}")]
        public async Task<IActionResult> Put([FromRoute] int syncLogId, [FromBody] SyncLogsResource syncLogResource)
        {
            if (syncLogResource == null)
                return BadRequest();


            SyncLogs? syncLogOld = await _syncLogService.GetAsync(syncLogId);
            if (syncLogOld == null)
                return NotFound();


            await _syncLogService.Update(syncLogOld, _mapper.Map<SyncLogs>(syncLogResource));
            return NoContent();
        }

        // DELETE /api/synclogs/{syncLogId}
        [HttpDelete("/api/synclogs/{syncLogId}")]
        public async Task<IActionResult> Delete([FromRoute] int syncLogId)
        {
            SyncLogs? syncLog = await _syncLogService.GetAsync(syncLogId);

            if (syncLog == null)
                return NotFound();
            try
            {
                await _syncLogService.DeleteAsync(syncLog);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { Error = e.Message });
            }

        }
    }
}
