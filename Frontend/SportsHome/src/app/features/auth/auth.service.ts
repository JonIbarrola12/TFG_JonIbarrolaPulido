import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ServicioAuth } from '../../core/services/auth.service';
import { PeticionLogin, PeticionRegistro, RespuestaAuth } from '../../core/models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class ServicioAuthFeature {
  private readonly servicioAuth = inject(ServicioAuth);

  iniciarSesion(credenciales: PeticionLogin): Observable<RespuestaAuth> {
    return this.servicioAuth.iniciarSesion(credenciales);
  }

  registrar(datos: PeticionRegistro): Observable<RespuestaAuth> {
    return this.servicioAuth.registrar(datos);
  }

  cerrarSesion(): void {
    this.servicioAuth.cerrarSesion();
  }
}
