import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ServicioAuth {

  private readonly apiUrl = environment.apiUrl;

  // 🔥 Estado reactivo del usuario
  private readonly usuario = signal<any | null>(null);
  readonly usuarioActual = this.usuario.asReadonly();

  // 🔥 Estado de autenticación
  private readonly autenticado = signal<boolean>(false);
  readonly estaAutenticado = this.autenticado.asReadonly();

  constructor(private http: HttpClient) {
    this.restaurarSesion();
  }

  // 🔐 LOGIN
  iniciarSesion(credenciales: any) {
    return this.http
      .post<any>(`${this.apiUrl}/Usuarios/login`, credenciales)
      .pipe(
        tap((usuario) => {
          this.usuario.set(usuario);
          this.autenticado.set(true);

          localStorage.setItem('usuario', JSON.stringify(usuario));
        })
      );
  }

  // 🆕 REGISTER
  registrar(datos: any) {
    return this.http.post(`${this.apiUrl}/Usuarios/register`, datos);
  }

  // 🚀 LOGOUT
  cerrarSesion() {
    this.usuario.set(null);
    this.autenticado.set(false);
    localStorage.removeItem('usuario');
  }

  // 🔄 RESTAURAR SESIÓN AL RECARGAR
  restaurarSesion() {
  const data = localStorage.getItem('usuario');

  if (data) {
    const usuario = JSON.parse(data);

    this.usuario.set(usuario);
    this.autenticado.set(true);
  } else {
    this.usuario.set(null);
    this.autenticado.set(false);
  }
}

  // ✏️ ACTUALIZACIÓN PARCIAL (PERFIL / AVATAR / ETC)
  actualizarUsuario(parcial: any) {
    const actual = this.usuario();

    if (!actual) return;

    const actualizado = {
      ...actual,
      ...parcial
    };

    this.usuario.set(actualizado);
    localStorage.setItem('usuario', JSON.stringify(actualizado));
  }

}
