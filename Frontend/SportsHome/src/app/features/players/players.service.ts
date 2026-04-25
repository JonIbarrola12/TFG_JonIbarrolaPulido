import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ServicioApi } from '../../core/services/api.service';
import { ENDPOINTS_API } from '../../core/constants/api.constants';
import { RespuestaApi } from '../../core/models/api-response.model';
import { Jugador } from '../../core/models/player.model';

@Injectable({
  providedIn: 'root'
})
export class ServicioJugadores {
  private readonly servicioApi = inject(ServicioApi);

  obtenerJugadores(): Observable<RespuestaApi<Jugador[]>> {
    return this.servicioApi.obtener<RespuestaApi<Jugador[]>>(ENDPOINTS_API.JUGADORES.BASE);
  }

  obtenerJugadorPorId(id: number): Observable<RespuestaApi<Jugador>> {
    return this.servicioApi.obtener<RespuestaApi<Jugador>>(ENDPOINTS_API.JUGADORES.POR_ID(id));
  }
}
