import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ServicioAuth } from '../auth.service';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  private readonly auth = inject(ServicioAuth);
  private readonly router = inject(Router);

  correo = '';
  contrasena = '';

  errorMessage: string = '';
  cargando = false;

  iniciarSesion(): void {
    this.cargando = true;
    this.errorMessage = '';

    this.auth.iniciarSesion({
      correo: this.correo,
      contrasena: this.contrasena
    }).subscribe({
      next: (res) => {
        this.cargando = false;
        this.router.navigate(['/']);
      },

      error: (err) => {
        this.cargando = false;
        this.errorMessage = this.obtenerMensajeError(err);
      }
    });
  }

  private obtenerMensajeError(err: any): string {

    // Si el backend manda error estructurado
    if (err?.error?.mensaje) {
      return err.error.mensaje;
    }

    // Códigos HTTP típicos
    if (err.status === 401) {
      return 'Credenciales incorrectas';
    }

    if (err.status === 400) {
      return 'Datos inválidos';
    }

    if (err.status === 409) {
      return 'El usuario ya existe';
    }

    if (err.status === 0) {
      return 'No se pudo conectar con el servidor';
    }

    return 'Ha ocurrido un error inesperado';
  }
}
