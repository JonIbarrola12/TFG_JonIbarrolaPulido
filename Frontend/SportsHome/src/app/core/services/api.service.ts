import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { RespuestaApi, RespuestaPaginada, ParametrosPaginacion } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class ServicioApi {
  private readonly urlBase = environment.apiUrl;
  private readonly http = inject(HttpClient);

  obtener<T>(endpoint: string, params?: Record<string, string | number | boolean>): Observable<T> {
    const parametrosHttp = this.construirParametros(params);
    return this.http.get<T>(`${this.urlBase}${endpoint}`, { params: parametrosHttp });
  }

  obtenerPaginado<T>(endpoint: string, paginacion: ParametrosPaginacion): Observable<RespuestaApi<RespuestaPaginada<T>>> {
    const params = {
      numeroPagina: paginacion.numeroPagina,
      tamanioPagina: paginacion.tamanioPagina,
      ...(paginacion.ordenarPor && { ordenarPor: paginacion.ordenarPor }),
      ...(paginacion.ordenDescendente !== undefined && { ordenDescendente: paginacion.ordenDescendente }),
      ...(paginacion.terminoBusqueda && { terminoBusqueda: paginacion.terminoBusqueda }),
    };
    return this.obtener<RespuestaApi<RespuestaPaginada<T>>>(endpoint, params);
  }

  crear<T>(endpoint: string, cuerpo: unknown): Observable<T> {
    return this.http.post<T>(`${this.urlBase}${endpoint}`, cuerpo);
  }

  actualizar<T>(endpoint: string, cuerpo: unknown): Observable<T> {
    return this.http.put<T>(`${this.urlBase}${endpoint}`, cuerpo);
  }

  actualizarParcial<T>(endpoint: string, cuerpo: unknown): Observable<T> {
    return this.http.patch<T>(`${this.urlBase}${endpoint}`, cuerpo);
  }

  eliminar<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.urlBase}${endpoint}`);
  }

  private construirParametros(params?: Record<string, string | number | boolean>): HttpParams {
    let parametrosHttp = new HttpParams();
    if (params) {
      Object.entries(params).forEach(([clave, valor]) => {
        if (valor !== null && valor !== undefined) {
          parametrosHttp = parametrosHttp.set(clave, String(valor));
        }
      });
    }
    return parametrosHttp;
  }
}
