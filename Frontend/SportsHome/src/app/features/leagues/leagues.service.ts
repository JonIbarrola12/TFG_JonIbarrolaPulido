import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ServicioApi } from '../../core/services/api.service';
import { ENDPOINTS_API } from '../../core/constants/api.constants';
import { RespuestaApi } from '../../core/models/api-response.model';
import { Liga } from '../../core/models/ligas.model';

type LigaApi = {
  LigaId: number;
  ExternalId: number;
  Nombre: string;
  Pais: string;
  Logo?: string;
};

type RespuestaLigas = RespuestaApi<LigaApi[]> | LigaApi[];

@Injectable({
  providedIn: 'root'
})
export class ServicioLigas {
  private readonly servicioApi = inject(ServicioApi);

  private mapLiga(liga: LigaApi): Liga {
    return {
      id: liga.LigaId,
      externalId: liga.ExternalId,
      nombre: liga.Nombre,
      pais: liga.Pais,
      urlLogo: liga.Logo,
    };
  }

  obtenerLigas(): Observable<Liga[]> {
    return this.servicioApi.obtener<RespuestaLigas>(ENDPOINTS_API.LIGAS.BASE).pipe(
      map((respuesta) => {
        const datos = Array.isArray(respuesta) ? respuesta : respuesta.datos ?? [];
        return datos.map((liga) => this.mapLiga(liga));
      })
    );
  }

  obtenerLigaPorId(id: number): Observable<Liga> {
    return this.servicioApi.obtener<RespuestaApi<Liga> | LigaApi>(ENDPOINTS_API.LIGAS.POR_ID(id)).pipe(
      map((respuesta) => {
        const liga = 'LigaId' in respuesta ? respuesta : respuesta.datos;
        return this.mapLiga(liga as LigaApi);
      })
    );
  }
  obtenerClasificacionPorLigaId(id: number): Observable<any[]> {
    return this.servicioApi
      .obtener<RespuestaApi<any> | any[]>(ENDPOINTS_API.LIGAS.CLASIFICACION(id))
      .pipe(
        map((respuesta) => {
          if (Array.isArray(respuesta)) {
            return respuesta;
          }

          if (respuesta && 'datos' in respuesta && Array.isArray(respuesta.datos)) {
            return respuesta.datos;
          }

          return [];
        })
      );
  }
  obtenerEquiposPorLigaId(id: number): Observable<any[]> {
    return this.servicioApi
      .obtener<RespuestaApi<any> | any[]>(ENDPOINTS_API.LIGAS.EQUIPOS(id))
      .pipe(
        map((respuesta) => {
          if (Array.isArray(respuesta)) {
            return respuesta;
          }

          if (respuesta && 'datos' in respuesta && Array.isArray(respuesta.datos)) {
            return respuesta.datos;
          }

          return [];
        })
      );
    }
  obtenerEquipoPorEquipoId(id: number): Observable<any> {
    return this.servicioApi.obtener<any>(ENDPOINTS_API.EQUIPOS.POR_ID(id)).pipe(
      map((respuesta: any) => {
        if (respuesta?.datos) return respuesta.datos;
        return respuesta;
      })
    );
  }
  obtenerJugadoresPorEquipoId(id: number): Observable<any[]> {
    return this.servicioApi
      .obtener<RespuestaApi<any> | any[]>(ENDPOINTS_API.EQUIPOS.JUGADORES(id))
      .pipe(
        map((respuesta) => {
          if (Array.isArray(respuesta)) {
            return respuesta;
          }

          if (respuesta && 'datos' in respuesta && Array.isArray(respuesta.datos)) {
            return respuesta.datos;
          }
          return [];
        })
      );
    }
    obtenerPartidosPorLigaId(id: number): Observable<any[]> {
      return this.servicioApi
      .obtener<RespuestaApi<any> | any[]>(ENDPOINTS_API.LIGAS.PARTIDOS(id))
      .pipe(
        map((respuesta) => {
          if (Array.isArray(respuesta)) {
            return respuesta;
          }

          if (respuesta && 'datos' in respuesta && Array.isArray(respuesta.datos)) {
            return respuesta.datos;
          }
          return [];
        })
      );
    }
}
