import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ServicioApi } from '../../core/services/api.service';
import { ENDPOINTS_API } from '../../core/constants/api.constants';
import { RespuestaApi } from '../../core/models/api-response.model';
import { Partido } from '../../core/models/match.model';

@Injectable({
  providedIn: 'root'
})
export class ServicioPartidos {
  private readonly servicioApi = inject(ServicioApi);

  obtenerPartidos(): Observable<RespuestaApi<Partido[]>> {
    return this.servicioApi.obtener<RespuestaApi<Partido[]>>(ENDPOINTS_API.PARTIDOS.BASE);
  }

  obtenerPartidoPorId(id: number): Observable<RespuestaApi<Partido>> {
    return this.servicioApi.obtener<RespuestaApi<Partido>>(ENDPOINTS_API.PARTIDOS.POR_ID(id));
  }
}
