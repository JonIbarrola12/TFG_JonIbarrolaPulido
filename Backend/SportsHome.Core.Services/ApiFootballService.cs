using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;

namespace SportsHome.Core.Services
{
    public class ApiFootballService : IApiFootball
    {
        private readonly HttpClient _httpClient;

        // Margen de seguridad: paramos 5 llamadas antes del limite real.
        // Asi nunca superamos el limite aunque haya pequeños desfases.
        private const int SafetyMargin = 5;

        // Llamadas restantes segun la API. Empezamos con valor conservador
        // hasta que la primera respuesta nos de el dato real.
        private int _remainingCalls = 90;
        private int _requestCount;

        public int RequestCount => _requestCount;
        public bool LimitReached => _remainingCalls <= SafetyMargin;

        public ApiFootballService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // 7 segundos entre peticiones = ~8.5 req/min (plan gratuito: 10 req/min)
        private static readonly TimeSpan ApiDelay = TimeSpan.FromSeconds(7);

        private static Task ThrottleAsync() => Task.Delay(ApiDelay);

        /// <summary>
        /// Hace una peticion GET y lee la cabecera x-ratelimit-requests-remaining
        /// para saber cuantas llamadas quedan REALMENTE en el plan diario.
        /// </summary>
        private async Task<T> GetWithRateLimitAsync<T>(string url)
        {
            if (LimitReached)
                throw new InvalidOperationException(
                    $"Limite diario alcanzado (quedan {_remainingCalls} llamadas, margen={SafetyMargin}). " +
                    $"Se han realizado {_requestCount} llamadas en esta sesion.");

            await ThrottleAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            _requestCount++;

            // Leer las llamadas restantes desde las cabeceras de la API
            if (response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var values))
            {
                if (int.TryParse(values.FirstOrDefault(), out int remaining))
                {
                    _remainingCalls = remaining;
                }
            }

            var content = await response.Content.ReadFromJsonAsync<T>();
            return content;
        }

        private void ThrowIfApiError<T>(ApiResponse<T> response, string endpoint)
        {
            if (response == null)
                throw new InvalidOperationException($"API Football devolvio null para {endpoint}");

            if (response.HasErrors)
            {
                var msg = string.Join("; ", response.Errors.EnumerateObject().Select(e => $"{e.Name}: {e.Value}"));
                throw new InvalidOperationException($"API Football error en {endpoint}: {msg}");
            }
        }

        // =========================
        // LIGAS + TEMPORADAS (1 sola llamada)
        // =========================
        public async Task<(List<Ligas> Ligas, List<LigasTemporadas> Temporadas)> GetLigasConTemporadasAsync()
        {
            var response = await GetWithRateLimitAsync<ApiResponse<List<LeagueWithSeasonsWrapper>>>("leagues");
            ThrowIfApiError(response, "leagues");
            var wrappers = response?.Response ?? new List<LeagueWithSeasonsWrapper>();

            var ligas = new List<Ligas>();
            var temporadas = new List<LigasTemporadas>();

            foreach (var w in wrappers)
            {
                ligas.Add(new Ligas
                {
                    ExternalId = w.League?.Id ?? 0,
                    Nombre = w.League?.Name,
                    Pais = w.Country?.Name,
                    Logo = w.League?.Logo
                });

                if (w.Seasons != null)
                {
                    foreach (var s in w.Seasons)
                    {
                        temporadas.Add(new LigasTemporadas
                        {
                            LigaId = w.League?.Id ?? 0,
                            Temporada = s.Year
                        });
                    }
                }
            }

            return (ligas, temporadas);
        }

        // =========================
        // EQUIPOS
        // =========================
        public async Task<List<Equipos>> GetEquiposAsync(int leagueId, int season)
        {
            var response = await GetWithRateLimitAsync<ApiResponse<List<TeamWrapper>>>(
                $"teams?league={leagueId}&season={season}");
            ThrowIfApiError(response, "teams");
            var wrappers = response?.Response ?? new List<TeamWrapper>();

            var result = new List<Equipos>();
            foreach (var w in wrappers)
            {
                result.Add(new Equipos
                {
                    ExternalId = w.Team?.Id ?? 0,
                    Nombre = w.Team?.Name,
                    Pais = w.Team?.Country,
                    Logo = w.Team?.Logo,
                    Fundacion = w.Team?.Founded,
                    NombreEstadio = w.Venue?.Name,
                    CapacidadEstadio = w.Venue?.Capacity
                });
            }

            return result;
        }

        // =========================
        // JUGADORES + ESTADISTICAS (1 sola llamada paginada)
        // El endpoint players?team=X&season=Y devuelve AMBOS datos.
        // =========================
        public async Task<(List<Jugadores> Jugadores, List<EstadisticasJugadores> Estadisticas)> GetJugadoresConEstadisticasAsync(int teamId, int season)
        {
            var allWrappers = new List<PlayerStatsWrapper>();
            int page = 1;
            int totalPages = 1;

            while (page <= totalPages)
            {
                var response = await GetWithRateLimitAsync<ApiResponse<List<PlayerStatsWrapper>>>(
                    $"players?team={teamId}&season={season}&page={page}");
                ThrowIfApiError(response, "players");
                allWrappers.AddRange(response?.Response ?? new List<PlayerStatsWrapper>());

                if (response?.Paging != null)
                    totalPages = response.Paging.Total;

                page++;
            }

            var jugadores = new List<Jugadores>();
            var estadisticas = new List<EstadisticasJugadores>();

            foreach (var w in allWrappers)
            {
                jugadores.Add(new Jugadores
                {
                    ExternalId = w.Player?.Id ?? 0,
                    Nombre = w.Player?.Name,
                    NombrePropio = w.Player?.Firstname,
                    Apellido = w.Player?.Lastname,
                    Edad = w.Player?.Age,
                    Nacionalidad = w.Player?.Nationality,
                    Altura = w.Player?.Height,
                    Peso = w.Player?.Weight,
                    Foto = w.Player?.Photo
                });

                if (w.Statistics != null)
                {
                    foreach (var stat in w.Statistics)
                    {
                        estadisticas.Add(new EstadisticasJugadores
                        {
                            JugadorId = w.Player?.Id ?? 0,
                            EquipoId = stat.Team?.Id ?? 0,
                            LigaId = stat.League?.Id ?? 0,
                            Temporada = season,
                            Apariciones = stat.Games?.Appearences,
                            Goles = stat.Goals?.Total,
                            Asistencias = stat.Goals?.Assists,
                            TarjetasAmarillas = stat.Cards?.Yellow,
                            TarjetasRojas = stat.Cards?.Red,
                            Minutos = stat.Games?.Minutes
                        });
                    }
                }
            }

            return (jugadores, estadisticas);
        }

        // =========================
        // PARTIDOS
        // =========================
        public async Task<List<Partidos>> GetPartidosAsync(int leagueId, int season)
        {
            var response = await GetWithRateLimitAsync<ApiResponse<List<FixtureWrapper>>>(
                $"fixtures?league={leagueId}&season={season}");
            ThrowIfApiError(response, "fixtures");
            var wrappers = response?.Response ?? new List<FixtureWrapper>();

            var result = new List<Partidos>();
            foreach (var w in wrappers)
            {
                result.Add(new Partidos
                {
                    ExternalId = w.Fixture?.Id ?? 0,
                    LigaId = leagueId,
                    Temporada = season,
                    Fecha = w.Fixture?.Date ?? DateTime.MinValue,
                    Estado = w.Fixture?.Status?.Short,
                    Ronda = w.League?.Round,
                    Arbitro = w.Fixture?.Referee,
                    ZonaHoraria = w.Fixture?.Timezone,
                    EquipoLocalId = w.Teams?.Home?.Id ?? 0,
                    EquipoVisitanteId = w.Teams?.Away?.Id ?? 0,
                    GolesLocal = w.Goals?.Home,
                    GolesVisitante = w.Goals?.Away
                });
            }

            return result;
        }

        // =========================
        // EVENTOS DE PARTIDOS
        // =========================
        public async Task<List<EventosPartidos>> GetEventosAsync(int fixtureId)
        {
            var response = await GetWithRateLimitAsync<ApiResponse<List<EventWrapper>>>(
                $"fixtures/events?fixture={fixtureId}");
            ThrowIfApiError(response, "fixtures/events");
            var wrappers = response?.Response ?? new List<EventWrapper>();

            var result = new List<EventosPartidos>();
            foreach (var w in wrappers)
            {
                result.Add(new EventosPartidos
                {
                    PartidoId = fixtureId,
                    EquipoId = w.Team?.Id ?? 0,
                    JugadorId = w.Player?.Id,
                    Minuto = w.Time?.Elapsed ?? 0,
                    Tipo = w.Type,
                    Detalle = w.Detail
                });
            }

            return result;
        }

        // =========================
        // CLASIFICACION
        // =========================
        public async Task<List<Clasificaciones>> GetClasificacionAsync(int leagueId, int season)
        {
            var response = await GetWithRateLimitAsync<ApiResponse<List<StandingWrapper>>>(
                $"standings?league={leagueId}&season={season}");
            ThrowIfApiError(response, "standings");
            var wrappers = response?.Response ?? new List<StandingWrapper>();

            var result = new List<Clasificaciones>();
            foreach (var sw in wrappers)
            {
                if (sw.League?.Standings == null) continue;

                foreach (var group in sw.League.Standings)
                {
                    foreach (var s in group)
                    {
                        result.Add(new Clasificaciones
                        {
                            LigaId = leagueId,
                            Temporada = season,
                            EquipoId = s.Team?.Id ?? 0,
                            Puntos = s.Points,
                            Posicion = s.Rank,
                            Jugados = s.All?.Played ?? 0,
                            Ganados = s.All?.Win ?? 0,
                            Empatados = s.All?.Draw ?? 0,
                            Perdidos = s.All?.Lose ?? 0,
                            GolesAFavor = s.All?.Goals?.For ?? 0,
                            GolesEnContra = s.All?.Goals?.Against ?? 0
                        });
                    }
                }
            }

            return result;
        }

        // =========================
        // Clases DTO internas para la API Football v3
        // =========================
        private class ApiResponse<T>
        {
            [JsonPropertyName("response")]
            public T Response { get; set; }

            [JsonPropertyName("errors")]
            public System.Text.Json.JsonElement Errors { get; set; }

            [JsonPropertyName("results")]
            public int Results { get; set; }

            [JsonPropertyName("paging")]
            public PagingDto Paging { get; set; }

            public bool HasErrors =>
                Errors.ValueKind == System.Text.Json.JsonValueKind.Object &&
                Errors.EnumerateObject().Any();
        }

        private class PagingDto
        {
            [JsonPropertyName("current")]
            public int Current { get; set; }

            [JsonPropertyName("total")]
            public int Total { get; set; }
        }

        // --- Leagues ---
        private class LeagueWithSeasonsWrapper
        {
            [JsonPropertyName("league")]
            public LeagueDto League { get; set; }
            [JsonPropertyName("country")]
            public CountryDto Country { get; set; }
            [JsonPropertyName("seasons")]
            public List<SeasonDto> Seasons { get; set; }
        }
        private class SeasonDto
        {
            [JsonPropertyName("year")]
            public int Year { get; set; }
        }
        private class LeagueDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("logo")]
            public string Logo { get; set; }
        }
        private class CountryDto
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }
        }

        // --- Teams ---
        private class TeamWrapper
        {
            [JsonPropertyName("team")]
            public TeamDto Team { get; set; }
            [JsonPropertyName("venue")]
            public VenueDto Venue { get; set; }
        }
        private class TeamDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("country")]
            public string Country { get; set; }
            [JsonPropertyName("logo")]
            public string Logo { get; set; }
            [JsonPropertyName("founded")]
            public int? Founded { get; set; }
        }
        private class VenueDto
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("capacity")]
            public int? Capacity { get; set; }
        }

        // --- Players + Statistics (mismo endpoint) ---
        private class PlayerStatsWrapper
        {
            [JsonPropertyName("player")]
            public PlayerDto Player { get; set; }
            [JsonPropertyName("statistics")]
            public List<PlayerStatDto> Statistics { get; set; }
        }
        private class PlayerDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("firstname")]
            public string Firstname { get; set; }
            [JsonPropertyName("lastname")]
            public string Lastname { get; set; }
            [JsonPropertyName("age")]
            public int? Age { get; set; }
            [JsonPropertyName("nationality")]
            public string Nationality { get; set; }
            [JsonPropertyName("height")]
            public string Height { get; set; }
            [JsonPropertyName("weight")]
            public string Weight { get; set; }
            [JsonPropertyName("photo")]
            public string Photo { get; set; }
        }
        private class PlayerStatDto
        {
            [JsonPropertyName("team")]
            public TeamRefDto Team { get; set; }
            [JsonPropertyName("league")]
            public StatLeagueDto League { get; set; }
            [JsonPropertyName("games")]
            public GamesDto Games { get; set; }
            [JsonPropertyName("goals")]
            public PlayerGoalsDto Goals { get; set; }
            [JsonPropertyName("cards")]
            public CardsDto Cards { get; set; }
        }
        private class StatLeagueDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
        }
        private class GamesDto
        {
            [JsonPropertyName("appearences")]
            public int? Appearences { get; set; }
            [JsonPropertyName("minutes")]
            public int? Minutes { get; set; }
        }
        private class PlayerGoalsDto
        {
            [JsonPropertyName("total")]
            public int? Total { get; set; }
            [JsonPropertyName("assists")]
            public int? Assists { get; set; }
        }
        private class CardsDto
        {
            [JsonPropertyName("yellow")]
            public int? Yellow { get; set; }
            [JsonPropertyName("red")]
            public int? Red { get; set; }
        }

        // --- Fixtures ---
        private class FixtureWrapper
        {
            [JsonPropertyName("fixture")]
            public FixtureDto Fixture { get; set; }
            [JsonPropertyName("league")]
            public FixtureLeagueDto League { get; set; }
            [JsonPropertyName("teams")]
            public FixtureTeamsDto Teams { get; set; }
            [JsonPropertyName("goals")]
            public GoalsDto Goals { get; set; }
        }
        private class FixtureDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("referee")]
            public string Referee { get; set; }
            [JsonPropertyName("timezone")]
            public string Timezone { get; set; }
            [JsonPropertyName("date")]
            public DateTime Date { get; set; }
            [JsonPropertyName("status")]
            public StatusDto Status { get; set; }
        }
        private class StatusDto
        {
            [JsonPropertyName("short")]
            public string Short { get; set; }
        }
        private class FixtureLeagueDto
        {
            [JsonPropertyName("round")]
            public string Round { get; set; }
        }
        private class FixtureTeamsDto
        {
            [JsonPropertyName("home")]
            public TeamRefDto Home { get; set; }
            [JsonPropertyName("away")]
            public TeamRefDto Away { get; set; }
        }
        private class TeamRefDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
        }
        private class GoalsDto
        {
            [JsonPropertyName("home")]
            public int? Home { get; set; }
            [JsonPropertyName("away")]
            public int? Away { get; set; }
        }

        // --- Events ---
        private class EventWrapper
        {
            [JsonPropertyName("time")]
            public TimeDto Time { get; set; }
            [JsonPropertyName("team")]
            public TeamRefDto Team { get; set; }
            [JsonPropertyName("player")]
            public PlayerRefDto Player { get; set; }
            [JsonPropertyName("type")]
            public string Type { get; set; }
            [JsonPropertyName("detail")]
            public string Detail { get; set; }
            [JsonPropertyName("comments")]
            public string Comments { get; set; }
        }
        private class TimeDto
        {
            [JsonPropertyName("elapsed")]
            public int? Elapsed { get; set; }
        }
        private class PlayerRefDto
        {
            [JsonPropertyName("id")]
            public int? Id { get; set; }
        }

        // --- Standings ---
        private class StandingWrapper
        {
            [JsonPropertyName("league")]
            public StandingLeagueDto League { get; set; }
        }
        private class StandingLeagueDto
        {
            [JsonPropertyName("standings")]
            public List<List<StandingEntryDto>> Standings { get; set; }
        }
        private class StandingEntryDto
        {
            [JsonPropertyName("rank")]
            public int Rank { get; set; }
            [JsonPropertyName("team")]
            public TeamRefDto Team { get; set; }
            [JsonPropertyName("points")]
            public int Points { get; set; }
            [JsonPropertyName("all")]
            public StandingAllDto All { get; set; }
        }
        private class StandingAllDto
        {
            [JsonPropertyName("played")]
            public int Played { get; set; }
            [JsonPropertyName("win")]
            public int Win { get; set; }
            [JsonPropertyName("draw")]
            public int Draw { get; set; }
            [JsonPropertyName("lose")]
            public int Lose { get; set; }
            [JsonPropertyName("goals")]
            public StandingGoalsDto Goals { get; set; }
        }
        private class StandingGoalsDto
        {
            [JsonPropertyName("for")]
            public int For { get; set; }
            [JsonPropertyName("against")]
            public int Against { get; set; }
        }
    }
}
