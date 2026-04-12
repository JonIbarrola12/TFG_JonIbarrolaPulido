export type EstadoPartido = 'PROGRAMADO' | 'EN_DIRECTO' | 'FINALIZADO' | 'APLAZADO' | 'CANCELADO';

export interface Partido {
  id: number;
  ligaId: number;
  nombreLiga?: string;
  equipoLocalId: number;
  nombreEquipoLocal?: string;
  urlLogoEquipoLocal?: string;
  equipoVisitanteId: number;
  nombreEquipoVisitante?: string;
  urlLogoEquipoVisitante?: string;
  golesLocal?: number;
  golesVisitante?: number;
  fechaPartido: string;
  estado: EstadoPartido;
  jornada?: string;
  temporada: number;
  estadio?: string;
  arbitro?: string;
}

export interface EventoPartido {
  id: number;
  partidoId: number;
  jugadorId: number;
  nombreJugador?: string;
  equipoId: number;
  tipo: 'GOL' | 'TARJETA_AMARILLA' | 'TARJETA_ROJA' | 'SUSTITUCION' | 'GOL_EN_PROPIA' | 'PENALTI';
  minuto: number;
  descripcion?: string;
}

export interface EstadisticasPartido {
  partidoId: number;
  equipoId: number;
  posesion: number;
  disparos: number;
  disparosAPuerta: number;
  corners: number;
  faltas: number;
  tarjetasAmarillas: number;
  tarjetasRojas: number;
  fuerasDeJuego: number;
  pases: number;
  precisionPases: number;
}
