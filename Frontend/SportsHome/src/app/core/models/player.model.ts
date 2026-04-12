export type PosicionJugador = 'POR' | 'DEF' | 'MED' | 'DEL';

export interface Jugador {
  id: number;
  nombre: string;
  apellidos: string;
  nombreCompleto?: string;
  fechaNacimiento?: string;
  nacionalidad?: string;
  posicion: PosicionJugador;
  numeroCamiseta?: number;
  urlFoto?: string;
  equipoId: number;
  nombreEquipo?: string;
  altura?: number;
  peso?: number;
  activo: boolean;
  creadoEn?: string;
  actualizadoEn?: string;
}

export interface EstadisticasJugador {
  jugadorId: number;
  temporada: number;
  partidosJugados: number;
  minutosJugados: number;
  goles: number;
  asistencias: number;
  tarjetasAmarillas: number;
  tarjetasRojas: number;
  precisionPases?: number;
  disparosAPuerta?: number;
  entradas?: number;
  interceptaciones?: number;
}
