import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
@Injectable({providedIn: 'root'})
export class UsuarioService {
  private readonly urlBase = environment.apiUrl;
  private readonly http = inject(HttpClient);

  updatePerfil(usuarioId: number, data: any) {
    return this.http.put(`${this.urlBase}/usuarios/perfil/${usuarioId}`, data);
  }

  getUsuario(id: number) {
    return this.http.get(`${this.urlBase}/usuarios/${id}`);
  }

  uploadAvatar(usuarioId: number, file: FormData) {
    return this.http.post(
      `${this.urlBase}/usuarios/upload-avatar/${usuarioId}`,
      file
    );
  }

}
