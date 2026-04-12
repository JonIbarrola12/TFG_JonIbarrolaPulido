export interface Equipo {
  id: number;
  nombre: string;
  nombreCorto?: string;
  urlLogo?: string;
  pais: string;
  ciudad?: string;
  estadio?: string;
  anioFundacion?: number;
  ligaId: number;
  nombreLiga?: string;
  creadoEn?: string;
  actualizadoEn?: string;
}

export interface EstadisticasEquipo {
  equipoId: number;
  temporada: number;
  partidosJugados: number;
  victorias: number;
  empates: number;
  derrotas: number;
  golesMarcados: number;
  golesEncajados: number;
  porteriasACero: number;
  tarjetasAmarillas: number;
  tarjetasRojas: number;
}
