import { Injectable, inject, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { ServicioApi } from './api.service';
import { ENDPOINTS_API, RUTAS_APP, CLAVES_ALMACENAMIENTO } from '../constants/api.constants';
import {
  RespuestaAuth,
  PeticionLogin,
  PeticionRegistro,
  PeticionRefrescarToken,
  Usuario
} from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class ServicioAuth {
  private readonly servicioApi = inject(ServicioApi);
  private readonly router = inject(Router);

  private readonly _usuarioActual = signal<Usuario | null>(this.cargarUsuarioDeStorage());
  private readonly _cargando = signal(false);

  readonly usuarioActual = this._usuarioActual.asReadonly();
  readonly cargando = this._cargando.asReadonly();
  readonly estaAutenticado = computed(() => !!this._usuarioActual());
  readonly esAdmin = computed(() => this._usuarioActual()?.rol === 'ADMIN');

  iniciarSesion(credenciales: PeticionLogin): Observable<RespuestaAuth> {
    this._cargando.set(true);
    return this.servicioApi.crear<RespuestaAuth>(ENDPOINTS_API.AUTH.LOGIN, credenciales).pipe(
      tap((respuesta) => {
        this.guardarSesion(respuesta);
        this._usuarioActual.set(respuesta.usuario);
        this._cargando.set(false);
        this.router.navigate([RUTAS_APP.INICIO]);
      }),
      catchError((error) => {
        this._cargando.set(false);
        return throwError(() => error);
      })
    );
  }

  registrar(datos: PeticionRegistro): Observable<RespuestaAuth> {
    this._cargando.set(true);
    return this.servicioApi.crear<RespuestaAuth>(ENDPOINTS_API.AUTH.REGISTRO, datos).pipe(
      tap((respuesta) => {
        this.guardarSesion(respuesta);
        this._usuarioActual.set(respuesta.usuario);
        this._cargando.set(false);
        this.router.navigate([RUTAS_APP.INICIO]);
      }),
      catchError((error) => {
        this._cargando.set(false);
        return throwError(() => error);
      })
    );
  }

  refrescarToken(): Observable<RespuestaAuth> {
    const tokenRefresco = this.obtenerTokenRefresco();
    if (!tokenRefresco) return throwError(() => new Error('No hay token de refresco disponible'));

    const cuerpo: PeticionRefrescarToken = { tokenRefresco };
    return this.servicioApi.crear<RespuestaAuth>(ENDPOINTS_API.AUTH.REFRESCAR, cuerpo).pipe(
      tap((respuesta) => {
        this.guardarSesion(respuesta);
        this._usuarioActual.set(respuesta.usuario);
      })
    );
  }

  cerrarSesion(): void {
    localStorage.removeItem(CLAVES_ALMACENAMIENTO.TOKEN_ACCESO);
    localStorage.removeItem(CLAVES_ALMACENAMIENTO.TOKEN_REFRESCO);
    localStorage.removeItem(CLAVES_ALMACENAMIENTO.USUARIO);
    this._usuarioActual.set(null);
    this.router.navigate([RUTAS_APP.LOGIN]);
  }

  obtenerTokenAcceso(): string | null {
    return localStorage.getItem(CLAVES_ALMACENAMIENTO.TOKEN_ACCESO);
  }

  obtenerTokenRefresco(): string | null {
    return localStorage.getItem(CLAVES_ALMACENAMIENTO.TOKEN_REFRESCO);
  }

  private guardarSesion(respuesta: RespuestaAuth): void {
    localStorage.setItem(CLAVES_ALMACENAMIENTO.TOKEN_ACCESO, respuesta.tokenAcceso);
    localStorage.setItem(CLAVES_ALMACENAMIENTO.TOKEN_REFRESCO, respuesta.tokenRefresco);
    localStorage.setItem(CLAVES_ALMACENAMIENTO.USUARIO, JSON.stringify(respuesta.usuario));
  }

  private cargarUsuarioDeStorage(): Usuario | null {
    const usuarioJson = localStorage.getItem(CLAVES_ALMACENAMIENTO.USUARIO);
    if (!usuarioJson) return null;
    try {
      return JSON.parse(usuarioJson) as Usuario;
    } catch {
      return null;
    }
  }
}
