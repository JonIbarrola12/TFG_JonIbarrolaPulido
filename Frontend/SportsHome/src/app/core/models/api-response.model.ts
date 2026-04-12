export interface RespuestaApi<T> {
  datos: T;
  exitoso: boolean;
  mensaje: string;
  errores?: string[];
  totalElementos?: number;
  numeroPagina?: number;
  tamanioPagina?: number;
}

export interface RespuestaPaginada<T> {
  elementos: T[];
  totalElementos: number;
  numeroPagina: number;
  tamanioPagina: number;
  totalPaginas: number;
  tienePaginaAnterior: boolean;
  tienePaginaSiguiente: boolean;
}

export interface ParametrosPaginacion {
  numeroPagina: number;
  tamanioPagina: number;
  ordenarPor?: string;
  ordenDescendente?: boolean;
  terminoBusqueda?: string;
}
