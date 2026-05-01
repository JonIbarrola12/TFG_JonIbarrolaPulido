import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ServicioAuth } from '../auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  private readonly auth = inject(ServicioAuth);
  private readonly router = inject(Router);

  // 🔥 TE FALTABAN ESTAS PROPIEDADES
  correo: string = '';
  nombreUsuario: string = '';
  contrasena: string = '';

  errorMessage: string = '';
  cargando: boolean = false;

  registrarse() {
    this.cargando = true;
    this.errorMessage = '';

    this.auth.registrar({
      correo: this.correo,
      nombreUsuario: this.nombreUsuario,
      contrasena: this.contrasena
    }).subscribe({
      next: () => {
        this.router.navigate(['/auth/login']);
      },
      error: (err) => {
        this.errorMessage = this.obtenerMensajeError(err);
        this.cargando = false;
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
