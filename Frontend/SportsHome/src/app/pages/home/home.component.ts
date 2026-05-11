import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HomeService } from '../../core/services/home.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  private readonly servicioHome = inject(HomeService);
  estadisticasList: any[] = [];

  ngOnInit(): void {

    this.servicioHome.obtenerEstadisticas()
      .subscribe({
        next: (data) => {
          this.estadisticasList = [
            {
              titulo: 'Mejores Jugadores',
              columna: 'G+A',
              valor: (j: any) => j.Goles + j.Asistencias,
              datos: data.topMejoresJugadores
            },
            {
              titulo: 'Jugadores Más Problemáticos',
              columna: 'Tarjetas',
              valor: (j: any) => j.TarjetasAmarillas + j.TarjetasRojas,
              datos: data.topMasProblematicos
            },
            {
              titulo: 'Jugadores Con Más Minutos',
              columna: 'Minutos',
              valor: (j: any) => j.Minutos,
              datos: data.topMinutosTotales
            }
          ];

        },
        error: (err) => {
          console.error(err);
        }
    });

  }

}
