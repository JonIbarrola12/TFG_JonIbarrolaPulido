export const ENDPOINTS_API = {
  // Autenticación
  AUTH: {
    LOGIN: '/auth/login',
    REGISTRO: '/auth/registro',
    REFRESCAR: '/auth/refrescar',
    LOGOUT: '/auth/logout',
    YO: '/auth/yo',
  },
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
    BASE: '/jugadores',
    POR_ID: (id: number) => `/jugadores/${id}`,
    ESTADISTICAS: (id: number) => `/jugadores/${id}/estadisticas`,
  },
  // Partidos
  PARTIDOS: {
    BASE: '/partidos',
    POR_ID: (id: number) => `/partidos/${id}`,
    EVENTOS: (id: number) => `/partidos/${id}/eventos`,
    ESTADISTICAS: (id: number) => `/partidos/${id}/estadisticas`,
    EN_DIRECTO: '/partidos/en-directo',
  },
} as const;

export const RUTAS_APP = {
  INICIO: '/',
  LIGAS: '/ligas',
  DETALLE_LIGA: (id: number) => `/ligas/${id}`,
  EQUIPOS: '/equipos',
  DETALLE_EQUIPO: (id: number) => `/equipos/${id}`,
  JUGADORES: '/jugadores',
  DETALLE_JUGADOR: (id: number) => `/jugadores/${id}`,
  PARTIDOS: '/partidos',
  DETALLE_PARTIDO: (id: number) => `/partidos/${id}`,
  LOGIN: '/auth/login',
  REGISTRO: '/auth/registro',
  PERFIL: '/perfil',
  NO_ENCONTRADO: '/404',
} as const;

export const CLAVES_ALMACENAMIENTO = {
  TOKEN_ACCESO: 'sh_token_acceso',
  TOKEN_REFRESCO: 'sh_token_refresco',
  USUARIO: 'sh_usuario',
  TEMA: 'sh_tema',
  IDIOMA: 'sh_idioma',
} as const;

export const PAGINACION = {
  PAGINA_POR_DEFECTO: 1,
  TAMANIO_PAGINA_POR_DEFECTO: 10,
  OPCIONES_TAMANIO: [5, 10, 25, 50],
} as const;
