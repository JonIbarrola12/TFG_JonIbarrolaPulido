using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SportsHome.Core.Interfaces.Services;

namespace SportsHome.UI.API
{
    /// <summary>
    /// Ejecuta una sincronización completa contra la API Football al arrancar la aplicación.
    /// Respeta los cooldowns definidos en SyncLogsService, por lo que solo hará
    /// llamadas reales si ha pasado el tiempo configurado desde la última sincronización.
    /// </summary>
    public class BackgroundSyncService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BackgroundSyncService> _logger;

        public BackgroundSyncService(IServiceScopeFactory scopeFactory, ILogger<BackgroundSyncService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BackgroundSyncService: iniciando sincronización automática al arrancar...");

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var syncService = scope.ServiceProvider.GetRequiredService<ISyncLogsService>();
                await syncService.SyncFullAsync();

                _logger.LogInformation("BackgroundSyncService: sincronización automática completada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BackgroundSyncService: error durante la sincronización automática.");
            }
        }
    }
}
