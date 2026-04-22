import { Component, inject, signal, Signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { ServicioAuth } from '../../core/services/auth.service';
import { Usuario } from '../../core/models/auth.model';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  private readonly servicioAuth = inject(ServicioAuth);

  readonly estaAutenticado: Signal<boolean> = this.servicioAuth.estaAutenticado;
  readonly usuarioActual: Signal<Usuario | null> = this.servicioAuth.usuarioActual;
  readonly menuAbierto = signal(false);

  alternarMenu(): void {
    this.menuAbierto.update(v => !v);
  }

  cerrarMenu(): void {
    this.menuAbierto.set(false);
  }

  cerrarSesion(): void {
    this.servicioAuth.cerrarSesion();
    this.cerrarMenu();
  }
}
