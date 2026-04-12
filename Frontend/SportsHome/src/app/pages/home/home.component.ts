import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  readonly secciones = [
    { icono: '🏆', titulo: 'Ligas', descripcion: 'Consulta clasificaciones y estadísticas de todas las ligas.', ruta: '/ligas' },
    { icono: '🛡️', titulo: 'Equipos', descripcion: 'Información detallada de equipos y sus plantillas.', ruta: '/equipos' },
    { icono: '⚽', titulo: 'Jugadores', descripcion: 'Estadísticas individuales por temporada.', ruta: '/jugadores' },
    { icono: '📅', titulo: 'Partidos', descripcion: 'Resultados, eventos y estadísticas por partido.', ruta: '/partidos' },
  ];
}
