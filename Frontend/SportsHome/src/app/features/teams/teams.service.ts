import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ServicioApi } from '../../core/services/api.service';
import { ENDPOINTS_API } from '../../core/constants/api.constants';
import { RespuestaApi } from '../../core/models/api-response.model';
import { Equipo } from '../../core/models/team.model';

@Injectable({
  providedIn: 'root'
})
export class ServicioEquipos {
  private readonly servicioApi = inject(ServicioApi);

  obtenerEquipos(): Observable<RespuestaApi<Equipo[]>> {
    return this.servicioApi.obtener<RespuestaApi<Equipo[]>>(ENDPOINTS_API.EQUIPOS.BASE);
  }

  obtenerEquipoPorId(id: number): Observable<RespuestaApi<Equipo>> {
    return this.servicioApi.obtener<RespuestaApi<Equipo>>(ENDPOINTS_API.EQUIPOS.POR_ID(id));
  }
}
