import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  private readonly urlBase = environment.apiUrl;
  private readonly http = inject(HttpClient);

  getTopMejoresJugadores() {
    return this.http.get<any[]>(
      `${this.urlBase}/jugadores/estadisticas/top-mejores-jugadores`
    );
  }

  getTopMasProblematicos() {
    return this.http.get<any[]>(
      `${this.urlBase}/jugadores/estadisticas/top-mas-problematicos`
    );
  }

  getTopMinutosTotales() {
    return this.http.get<any[]>(
      `${this.urlBase}/jugadores/estadisticas/top-minutos-totales`
    );
  }

  obtenerEstadisticas() {
    return forkJoin({
      topMejoresJugadores: this.getTopMejoresJugadores(),
      topMasProblematicos: this.getTopMasProblematicos(),
      topMinutosTotales: this.getTopMinutosTotales()
    });
  }
}
