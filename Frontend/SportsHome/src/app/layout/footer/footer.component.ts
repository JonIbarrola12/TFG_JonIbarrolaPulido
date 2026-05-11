import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ServicioAuth } from '../../features/auth/auth.service';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent {

  private readonly auth = inject(ServicioAuth);

  readonly currentYear = new Date().getFullYear();

  // 👇 usamos signals directamente
  usuario = this.auth.usuarioActual;
  estaAutenticado = this.auth.estaAutenticado;

  logout(): void {
    this.auth.cerrarSesion();
  }
}
