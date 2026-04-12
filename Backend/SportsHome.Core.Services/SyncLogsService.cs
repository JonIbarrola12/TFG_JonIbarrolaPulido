using AutoMapper;
using Microsoft.AspNet.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Services;
using SportsHome.Core.Queries.OData;
using SportsHome.IL.Repository.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.Core.Services
{
    public class SyncLogsService : ISyncLogsService
    {
        // Ligas a sincronizar (ExternalId de API Football).
        // 140=La Liga, 39=Premier, 135=Serie A, 78=Bundesliga, 61=Ligue 1
        private static readonly HashSet<int> LigasPermitidas = new() { 140, 39, 135, 78, 61 };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SportsHomeContext _context;
        private readonly IApiFootball _api;
        private readonly ILogger<SyncLogsService> _logger;

        public SyncLogsService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IApiFootball api,
            SportsHomeContext context,
            ILogger<SyncLogsService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _api = api;
            _context = context;
            _logger = logger;
        }

        // ================= CRUD =================

        public async Task<SyncLogs?> GetAsync(int id)
            => await _unitOfWork.SyncLogs.GetAsync(id);

        public async Task<IEnumerable<SyncLogs>> GetAllAsync()
            => await _unitOfWork.SyncLogs.GetListAsync();

        public async Task<OQuery<SyncLogs>> GetFilteredAsync(ODataQueryOptions options)
            => await _unitOfWork.SyncLogs.GetFilteredAsync(options);

        public async Task<OQuery<SyncLogs>> GetSyncLogsAsync(ODataQueryOptions options)
            => await _unitOfWork.SyncLogs.GetFilteredAsync(options);

        public async Task<SyncLogs?> AddAsync(SyncLogs entity)
        {
            await _unitOfWork.SyncLogs.AddAsync(entity);
            await _unitOfWork.Complete();
            return entity;
        }

        public async Task Update(SyncLogs oldEntity, SyncLogs newEntity)
        {
            _mapper.Map(newEntity, oldEntity);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(SyncLogs entity)
        {
            await _unitOfWork.SyncLogs.RemoveAsync(entity);
            await _unitOfWork.Complete();
        }

        public async Task<IEnumerable<SyncLogs>> GetListAsync()
            => await _unitOfWork.SyncLogs.GetListAsync();

        // ================= SYNC =================

        /// <summary>
        /// Orquesta la sincronizacion completa respetando el limite de 100 llamadas/dia.
        ///
        /// Prioridad de sincronizacion (datos estructurales primero):
        ///   1. Ligas + Temporadas  (1 llamada)
        ///   2. Equipos             (5 llamadas, 1/liga)
        ///   3. Partidos            (5 llamadas, 1/liga)
        ///   4. Clasificaciones     (5 llamadas, 1/liga)
        ///   5. Jugadores + Stats   (~2-3 llamadas/equipo, se reparten entre dias)
        ///
        /// Cooldowns (horas) — evitan repetir llamadas innecesarias:
        ///   Ligas/Temporadas = 168h (7 dias)
        ///   Equipos          = 168h (7 dias)
        ///   Partidos         =   6h (se actualizan resultados)
        ///   Clasificaciones  =   6h
        ///   Jugadores+Stats  = 168h (7 dias)
        /// </summary>
        public async Task SyncFullAsync()
        {
            _logger.LogInformation("SyncFull: iniciando sincronizacion completa (llamadas usadas: {Count})...", _api.RequestCount);

            // 1. Ligas + Temporadas (1 sola llamada API)
            try
            {
                _logger.LogInformation("SyncFull [1/5]: Sincronizando ligas y temporadas...");
                await SyncLigasYTemporadasAsync();
                _logger.LogInformation("SyncFull [1/5]: Ligas y temporadas OK.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SyncFull [1/5]: Error sincronizando ligas/temporadas.");
                var hayLigas = await _context.Ligas.AnyAsync();
                if (!hayLigas)
                {
                    _logger.LogError("SyncFull: no hay ligas en BD y la API fallo. Abortando sync.");
                    return;
                }
                _logger.LogWarning("SyncFull: API fallo pero hay ligas en BD. Continuando con datos existentes.");
            }

            var ligas = await _context.Ligas
                .Where(l => LigasPermitidas.Contains(l.ExternalId))
                .ToListAsync();

            var temporada = DateTime.UtcNow.Year - 1; // temporada actual (ej: 2025 -> 2024)

            // FASE 1: Datos estructurales (equipos + partidos + clasificaciones)
            // Coste: ~16 llamadas para las 5 ligas (si no estan en cooldown)
            foreach (var liga in ligas)
            {
                if (_api.LimitReached)
                {
                    _logger.LogWarning("SyncFull: limite diario alcanzado ({Count} llamadas). Continuara en la proxima ejecucion.", _api.RequestCount);
                    return;
                }

                _logger.LogInformation("SyncFull: Procesando liga {Nombre} (ExternalId={ExtId})...",
                    liga.Nombre, liga.ExternalId);

                // 2. Equipos de la liga
                List<int> equipoExternalIds;
                try
                {
                    _logger.LogInformation("SyncFull [2/5]: Equipos de liga {ExtId}...", liga.ExternalId);
                    equipoExternalIds = await SyncEquiposAsync(liga.ExternalId, temporada);
                    _logger.LogInformation("SyncFull [2/5]: Equipos OK ({Count}).", equipoExternalIds.Count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SyncFull [2/5]: Error sincronizando equipos de liga {ExtId}.", liga.ExternalId);
                    equipoExternalIds = await _context.Equipos.Select(e => e.ExternalId).ToListAsync();
                    if (equipoExternalIds.Count == 0)
                    {
                        _logger.LogWarning("SyncFull [2/5]: No hay equipos en BD para liga {ExtId}. Saltando liga.", liga.ExternalId);
                        continue;
                    }
                    _logger.LogInformation("SyncFull [2/5]: Usando {Count} equipos existentes en BD.", equipoExternalIds.Count);
                }

                // 3. Partidos
                try
                {
                    _logger.LogInformation("SyncFull [3/5]: Partidos de liga {ExtId}...", liga.ExternalId);
                    await SyncPartidosAsync(liga.ExternalId, temporada);
                    _logger.LogInformation("SyncFull [3/5]: Partidos OK.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SyncFull [3/5]: Error sincronizando partidos.");
                }

                // 4. Clasificacion
                try
                {
                    _logger.LogInformation("SyncFull [4/5]: Clasificacion de liga {ExtId}...", liga.ExternalId);
                    await SyncClasificacionAsync(liga.ExternalId, temporada);
                    _logger.LogInformation("SyncFull [4/5]: Clasificacion OK.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SyncFull [4/5]: Error sincronizando clasificacion.");
                }
            }

            // FASE 2: Jugadores + Estadisticas (lo mas costoso, se reparte entre dias)
            // Cada equipo = ~2-3 llamadas. 100 equipos = ~250 llamadas.
            // Con el contador, paramos al llegar al limite.
            foreach (var liga in ligas)
            {
                if (_api.LimitReached)
                {
                    _logger.LogWarning("SyncFull: limite diario alcanzado antes de jugadores ({Count} llamadas). Continuara manana.", _api.RequestCount);
                    return;
                }

                var equipoExternalIds = await _context.Equipos
                    .Select(e => e.ExternalId)
                    .ToListAsync();

                var equipos = await _context.Equipos
                    .Where(e => equipoExternalIds.Contains(e.ExternalId))
                    .ToListAsync();

                _logger.LogInformation("SyncFull [5/5]: Jugadores/stats para {Count} equipos de liga {Nombre}...", equipos.Count, liga.Nombre);

                foreach (var equipo in equipos)
                {
                    if (_api.LimitReached)
                    {
                        _logger.LogWarning("SyncFull: limite diario alcanzado en equipo {ExtId} ({Count} llamadas). Continuara manana.",
                            equipo.ExternalId, _api.RequestCount);
                        return;
                    }

                    try
                    {
                        await SyncJugadoresYEstadisticasAsync(equipo.ExternalId, temporada);
                    }
                    catch (Exception ex)
                    {
                        if (_api.LimitReached)
                        {
                            _logger.LogWarning("SyncFull: limite alcanzado. Parando.");
                            return;
                        }
                        _logger.LogError(ex, "SyncFull [5/5]: Error en equipo {ExtId}.", equipo.ExternalId);
                        _context.ChangeTracker.Clear();
                    }
                }
            }

            _logger.LogInformation("SyncFull: sincronizacion completa finalizada. Total llamadas API: {Count}.", _api.RequestCount);
        }

        // ================= LIGAS + TEMPORADAS (1 llamada) =================

        public async Task SyncLigasYTemporadasAsync()
        {
            bool skipLigas = await ShouldSkip("Leagues", 168);
            bool skipTemps = await ShouldSkip("Seasons", 168);

            if (skipLigas && skipTemps)
            {
                _logger.LogInformation("SyncLigasYTemporadas: SKIP (cooldown activo para ambos).");
                return;
            }

            _logger.LogInformation("SyncLigasYTemporadas: llamando a API Football (leagues)...");
            var (ligasApi, temporadasApi) = await _api.GetLigasConTemporadasAsync();
            _logger.LogInformation("SyncLigasYTemporadas: API devolvio {LigaCount} ligas y {TempCount} temporadas.", ligasApi.Count, temporadasApi.Count);

            // Guardar ligas
            if (!skipLigas)
            {
                foreach (var dto in ligasApi.Where(l => LigasPermitidas.Contains(l.ExternalId)))
                {
                    var liga = await _context.Ligas
                        .FirstOrDefaultAsync(l => l.ExternalId == dto.ExternalId);

                    if (liga == null)
                    {
                        _context.Ligas.Add(new Ligas
                        {
                            ExternalId = dto.ExternalId,
                            Nombre = dto.Nombre,
                            Pais = dto.Pais,
                            Logo = dto.Logo
                        });
                    }
                }

                await SaveSync("Leagues");
            }

            // Guardar temporadas
            if (!skipTemps)
            {
                foreach (var dto in temporadasApi.Where(t => LigasPermitidas.Contains(t.LigaId)))
                {
                    var liga = await _context.Ligas
                        .FirstOrDefaultAsync(l => l.ExternalId == dto.LigaId);

                    if (liga == null) continue;

                    var existe = await _context.LigasTemporadas.AnyAsync(lt =>
                        lt.LigaId == liga.LigaId && lt.Temporada == dto.Temporada);

                    if (!existe)
                    {
                        _context.LigasTemporadas.Add(new LigasTemporadas
                        {
                            LigaId = liga.LigaId,
                            Temporada = dto.Temporada
                        });
                    }
                }

                await SaveSync("Seasons");
            }
        }

        // ================= EQUIPOS =================

        public async Task<List<int>> SyncEquiposAsync(int ligaExternalId, int temporada)
        {
            if (await ShouldSkip($"Teams_{ligaExternalId}_{temporada}", 168))
            {
                _logger.LogInformation("SyncEquipos: SKIP liga {LigaId} (cooldown). Leyendo ExternalIds de BD.", ligaExternalId);
                // Devolver los ExternalIds desde BD sin gastar llamada API
                return await _context.Equipos
                    .Select(e => e.ExternalId)
                    .ToListAsync();
            }

            _logger.LogInformation("SyncEquipos: llamando a API Football (teams) para liga {LigaId}, temporada {Temp}...", ligaExternalId, temporada);
            var equiposApi = await _api.GetEquiposAsync(ligaExternalId, temporada);
            _logger.LogInformation("SyncEquipos: API devolvio {Count} equipos para liga {LigaId}.", equiposApi.Count, ligaExternalId);

            foreach (var dto in equiposApi)
            {
                var equipo = await _context.Equipos
                    .FirstOrDefaultAsync(e => e.ExternalId == dto.ExternalId);

                if (equipo == null)
                {
                    _context.Equipos.Add(new Equipos
                    {
                        ExternalId = dto.ExternalId,
                        Nombre = dto.Nombre,
                        Pais = dto.Pais,
                        Logo = dto.Logo,
                        Fundacion = dto.Fundacion,
                        NombreEstadio = dto.NombreEstadio,
                        CapacidadEstadio = dto.CapacidadEstadio
                    });
                }
                else
                {
                    equipo.Nombre = dto.Nombre;
                    equipo.Pais = dto.Pais;
                    equipo.Logo = dto.Logo;
                    equipo.Fundacion = dto.Fundacion;
                    equipo.NombreEstadio = dto.NombreEstadio;
                    equipo.CapacidadEstadio = dto.CapacidadEstadio;
                }
            }

            await SaveSync($"Teams_{ligaExternalId}_{temporada}");

            return equiposApi.Select(e => e.ExternalId).ToList();
        }

        // ================= JUGADORES + ESTADISTICAS (1 llamada por equipo) =================

        public async Task SyncJugadoresYEstadisticasAsync(int equipoExternalId, int temporada)
        {
            bool skipJugadores = await ShouldSkip($"Players_{equipoExternalId}_{temporada}", 168);
            bool skipStats = await ShouldSkip($"Stats_{equipoExternalId}_{temporada}", 168);

            if (skipJugadores && skipStats)
            {
                _logger.LogInformation("SyncJugadoresYStats: SKIP equipo {ExtId} (cooldown para ambos).", equipoExternalId);
                return;
            }

            _logger.LogInformation("SyncJugadoresYStats: llamando a API para equipo {ExtId}...", equipoExternalId);
            var (jugadoresApi, statsApi) = await _api.GetJugadoresConEstadisticasAsync(equipoExternalId, temporada);
            _logger.LogInformation("SyncJugadoresYStats: API devolvio {JCount} jugadores y {SCount} stats para equipo {ExtId}.",
                jugadoresApi.Count, statsApi.Count, equipoExternalId);

            var equipo = await _context.Equipos
                .FirstOrDefaultAsync(e => e.ExternalId == equipoExternalId);

            if (equipo == null) return;

            // Guardar jugadores
            if (!skipJugadores && jugadoresApi.Count > 0)
            {
                foreach (var dto in jugadoresApi)
                {
                    var jugador = await _context.Jugadores
                        .FirstOrDefaultAsync(j => j.ExternalId == dto.ExternalId);

                    if (jugador == null)
                    {
                        jugador = new Jugadores
                        {
                            ExternalId = dto.ExternalId,
                            Nombre = dto.Nombre,
                            NombrePropio = dto.NombrePropio,
                            Apellido = dto.Apellido,
                            Edad = dto.Edad,
                            Nacionalidad = dto.Nacionalidad,
                            Altura = dto.Altura,
                            Peso = dto.Peso,
                            Foto = dto.Foto
                        };

                        _context.Jugadores.Add(jugador);
                    }

                    var existe = await _context.JugadoresEquipos.AnyAsync(je =>
                        je.JugadorId == jugador.JugadorId &&
                        je.EquipoId == equipo.EquipoId &&
                        je.Temporada == temporada);

                    if (!existe)
                    {
                        _context.JugadoresEquipos.Add(new JugadoresEquipos
                        {
                            Jugador = jugador,
                            Equipo = equipo,
                            Temporada = temporada
                        });
                    }
                }

                await SaveSync($"Players_{equipoExternalId}_{temporada}");
            }

            // Guardar estadisticas
            if (!skipStats && statsApi.Count > 0)
            {
                foreach (var dto in statsApi)
                {
                    var jugador = await _context.Jugadores
                        .FirstOrDefaultAsync(j => j.ExternalId == dto.JugadorId);

                    var equipoStat = await _context.Equipos
                        .FirstOrDefaultAsync(e => e.ExternalId == dto.EquipoId);

                    var liga = await _context.Ligas
                        .FirstOrDefaultAsync(l => l.ExternalId == dto.LigaId);

                    if (jugador == null || equipoStat == null || liga == null) continue;

                    var estadistica = await _context.EstadisticasJugadores
                        .FirstOrDefaultAsync(ej =>
                            ej.JugadorId == jugador.JugadorId &&
                            ej.EquipoId == equipoStat.EquipoId &&
                            ej.LigaId == liga.LigaId &&
                            ej.Temporada == temporada);

                    if (estadistica == null)
                    {
                        _context.EstadisticasJugadores.Add(new EstadisticasJugadores
                        {
                            JugadorId = jugador.JugadorId,
                            EquipoId = equipoStat.EquipoId,
                            LigaId = liga.LigaId,
                            Temporada = temporada,
                            Apariciones = dto.Apariciones,
                            Goles = dto.Goles,
                            Asistencias = dto.Asistencias,
                            TarjetasAmarillas = dto.TarjetasAmarillas,
                            TarjetasRojas = dto.TarjetasRojas,
                            Minutos = dto.Minutos
                        });
                    }
                    else
                    {
                        estadistica.Apariciones = dto.Apariciones;
                        estadistica.Goles = dto.Goles;
                        estadistica.Asistencias = dto.Asistencias;
                        estadistica.TarjetasAmarillas = dto.TarjetasAmarillas;
                        estadistica.TarjetasRojas = dto.TarjetasRojas;
                        estadistica.Minutos = dto.Minutos;
                    }
                }

                await SaveSync($"Stats_{equipoExternalId}_{temporada}");
            }
        }

        // ================= PARTIDOS =================

        public async Task SyncPartidosAsync(int ligaExternalId, int temporada)
        {
            if (await ShouldSkip($"Fixtures_{ligaExternalId}_{temporada}", 6))
            {
                _logger.LogInformation("SyncPartidos: SKIP liga {ExtId} (cooldown).", ligaExternalId);
                return;
            }

            _logger.LogInformation("SyncPartidos: llamando a API para liga {ExtId}, temporada {Temp}...", ligaExternalId, temporada);
            var partidosApi = await _api.GetPartidosAsync(ligaExternalId, temporada);
            _logger.LogInformation("SyncPartidos: API devolvio {Count} partidos para liga {ExtId}.", partidosApi.Count, ligaExternalId);

            if (partidosApi.Count == 0) return;

            var liga = await _context.Ligas
                .FirstOrDefaultAsync(l => l.ExternalId == ligaExternalId);

            if (liga == null) return;

            var equipos = await _context.Equipos.ToListAsync();

            foreach (var dto in partidosApi)
            {
                var partido = await _context.Partidos
                    .FirstOrDefaultAsync(p => p.ExternalId == dto.ExternalId);

                var local = equipos.FirstOrDefault(e => e.ExternalId == dto.EquipoLocalId);
                var visitante = equipos.FirstOrDefault(e => e.ExternalId == dto.EquipoVisitanteId);

                if (local == null || visitante == null) continue;

                if (partido == null)
                {
                    _context.Partidos.Add(new Partidos
                    {
                        ExternalId = dto.ExternalId,
                        LigaId = liga.LigaId,
                        Temporada = temporada,
                        Fecha = dto.Fecha,
                        Estado = dto.Estado,
                        Ronda = dto.Ronda,
                        Arbitro = dto.Arbitro,
                        ZonaHoraria = dto.ZonaHoraria,
                        EquipoLocalId = local.EquipoId,
                        EquipoVisitanteId = visitante.EquipoId,
                        GolesLocal = dto.GolesLocal,
                        GolesVisitante = dto.GolesVisitante
                    });
                }
                else
                {
                    partido.Estado = dto.Estado;
                    partido.GolesLocal = dto.GolesLocal;
                    partido.GolesVisitante = dto.GolesVisitante;
                }
            }

            await SaveSync($"Fixtures_{ligaExternalId}_{temporada}");
        }

        // ================= EVENTOS =================

        public async Task SyncEventosAsync(int partidoExternalId)
        {
            if (await ShouldSkip($"Events_{partidoExternalId}", 1)) return;

            var eventosApi = await _api.GetEventosAsync(partidoExternalId);

            var partido = await _context.Partidos
                .FirstOrDefaultAsync(p => p.ExternalId == partidoExternalId);

            if (partido == null) return;

            foreach (var dto in eventosApi)
            {
                var existe = await _context.EventosPartidos.AnyAsync(e =>
                    e.PartidoId == partido.PartidoId &&
                    e.Minuto == dto.Minuto &&
                    e.Tipo == dto.Tipo &&
                    e.Detalle == dto.Detalle);

                if (existe) continue;

                var equipo = await _context.Equipos
                    .FirstOrDefaultAsync(e => e.ExternalId == dto.EquipoId);

                if (equipo == null) continue;

                int? jugadorId = null;

                if (dto.JugadorId != null)
                {
                    var jugador = await _context.Jugadores
                        .FirstOrDefaultAsync(j => j.ExternalId == dto.JugadorId);

                    jugadorId = jugador?.JugadorId;
                }

                _context.EventosPartidos.Add(new EventosPartidos
                {
                    PartidoId = partido.PartidoId,
                    EquipoId = equipo.EquipoId,
                    JugadorId = jugadorId,
                    Minuto = dto.Minuto,
                    Tipo = dto.Tipo,
                    Detalle = dto.Detalle
                });
            }

            await SaveSync($"Events_{partidoExternalId}");
        }

        // ================= CLASIFICACION =================

        public async Task SyncClasificacionAsync(int ligaExternalId, int temporada)
        {
            if (await ShouldSkip($"Standings_{ligaExternalId}_{temporada}", 6))
            {
                _logger.LogInformation("SyncClasificacion: SKIP liga {ExtId} (cooldown).", ligaExternalId);
                return;
            }

            _logger.LogInformation("SyncClasificacion: llamando a API para liga {ExtId}, temporada {Temp}...", ligaExternalId, temporada);
            var tablaApi = await _api.GetClasificacionAsync(ligaExternalId, temporada);
            _logger.LogInformation("SyncClasificacion: API devolvio {Count} entradas para liga {ExtId}.", tablaApi.Count, ligaExternalId);

            if (tablaApi.Count == 0) return;

            var liga = await _context.Ligas
                .FirstOrDefaultAsync(l => l.ExternalId == ligaExternalId);

            if (liga == null) return;

            foreach (var dto in tablaApi)
            {
                var equipo = await _context.Equipos
                    .FirstOrDefaultAsync(e => e.ExternalId == dto.EquipoId);

                if (equipo == null) continue;

                var clasificacion = await _context.Clasificaciones
                    .FirstOrDefaultAsync(c =>
                        c.LigaId == liga.LigaId &&
                        c.EquipoId == equipo.EquipoId &&
                        c.Temporada == temporada);

                if (clasificacion == null)
                {
                    _context.Clasificaciones.Add(new Clasificaciones
                    {
                        LigaId = liga.LigaId,
                        EquipoId = equipo.EquipoId,
                        Temporada = temporada,
                        Puntos = dto.Puntos,
                        Posicion = dto.Posicion,
                        Jugados = dto.Jugados,
                        Ganados = dto.Ganados,
                        Empatados = dto.Empatados,
                        Perdidos = dto.Perdidos,
                        GolesAFavor = dto.GolesAFavor,
                        GolesEnContra = dto.GolesEnContra
                    });
                }
                else
                {
                    clasificacion.Puntos = dto.Puntos;
                    clasificacion.Posicion = dto.Posicion;
                }
            }

            await SaveSync($"Standings_{ligaExternalId}_{temporada}");
        }

        // ================= HELPERS =================

        private async Task<bool> ShouldSkip(string entidad, int horas)
        {
            var sync = await _context.SyncLogs
                .FirstOrDefaultAsync(s => s.Entidad == entidad);

            return sync != null &&
                   sync.UltimaSincronizacion > DateTime.UtcNow.AddHours(-horas);
        }

        private async Task SaveSync(string entidad)
        {
            var sync = await _context.SyncLogs
                .FirstOrDefaultAsync(s => s.Entidad == entidad);

            if (sync == null)
            {
                _context.SyncLogs.Add(new SyncLogs
                {
                    Entidad = entidad,
                    UltimaSincronizacion = DateTime.UtcNow
                });
            }
            else
            {
                sync.UltimaSincronizacion = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
