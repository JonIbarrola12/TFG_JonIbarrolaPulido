import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ServicioPartidos {
  private readonly http = inject(HttpClient);
    private readonly urlBase = environment.apiUrl;

    getPartidos() {
      return this.http.get<any>(`${this.urlBase}/Partidos/todos`);
    }

  getPartidosFiltrados(filtros: any) {
    return this.http.get<any>(`${this.urlBase}/Partidos`, {
      params: filtros
    });
  }

  getEquipos(){
    return this.http.get<any>(`${this.urlBase}/Equipos`);
  }
}
