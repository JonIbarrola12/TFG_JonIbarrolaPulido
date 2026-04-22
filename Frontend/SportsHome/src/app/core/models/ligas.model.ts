export interface Liga {
  id: number;
  nombre: string;
  pais: string;
  urlLogo?: string;
  temporada: number;
  activa: boolean;
  creadaEn?: string;
  actualizadaEn?: string;
}

export interface ClasificacionLiga {
  posicion: number;
  equipoId: number;
  nombreEquipo: string;
  urlLogoEquipo?: string;
  jugados: number;
  ganados: number;
  empatados: number;
  perdidos: number;
  golesFavor: number;
  golesContra: number;
  diferenciaGoles: number;
  puntos: number;
}
