export const ENDPOINTS_API = {
  // Ligas
  LIGAS: {
    BASE: '/Ligas',
    POR_ID: (id: number) => `/Ligas/${id}`,
    CLASIFICACION: (id: number) => `/Ligas/${id}/clasificacion`,
    PARTIDOS: (id: number) => `/Ligas/${id}/partidos`,
    EQUIPOS: (id: number) => `/Ligas/${id}/equipos`,
  },
  // Equipos
  EQUIPOS: {
    BASE: '/Equipos',
    POR_ID: (id: number) => `/Equipos/${id}`,
    JUGADORES: (id: number) => `/Equipos/${id}/jugadores`,
    ESTADISTICAS: (id: number) => `/Equipos/${id}/estadisticas`,
    PARTIDOS: (id: number) => `/Equipos/${id}/partidos`,
  },
  // Jugadores
  JUGADORES: {
    BASE: '/Jugadores',
    POR_ID: (id: number) => `/Jugadores/${id}`,
    ESTADISTICAS: (id: number) => `/Jugadores/${id}/estadisticas`,
    ESTADISTICASLIGAS: (id: number) => `/Jugadores/estadisticas/ligas/${id}`,
  },
  // Partidos
  PARTIDOS: {
    BASE: '/Partidos',
    POR_ID: (id: number) => `/Partidos/${id}`,
    EVENTOS: (id: number) => `/Partidos/${id}/eventos`,
    ESTADISTICAS: (id: number) => `/Partidos/${id}/estadisticas`,
    EN_DIRECTO: '/Partidos/en-directo',
  },
} as const;
