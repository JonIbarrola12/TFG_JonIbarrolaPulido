import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ServicioLigas } from '../leagues.service';

@Component({
  selector: 'app-league-players',
  imports: [RouterModule],
  templateUrl: './league-players-stadistics.component.html',
  styleUrl: './league-players-stadistics.component.css'
})

export class LeaguePlayersStadisticsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly servicioLigas = inject(ServicioLigas);

    ligaId: string | null = null;
    equipoId: string | null = null;
    jugadorId: string | null = null;
    equipo: any;
    estadisticasJugador: any[] = [];

    cargando = true;
    errorMessage = '';



  ngOnInit(): void {
    this.jugadorId = this.route.snapshot.paramMap.get('jugadorId');
    this.equipoId = this.route.snapshot.paramMap.get('equipoId');
    this.ligaId = this.route.snapshot.paramMap.get('id');

    if (!this.ligaId) {
      this.errorMessage = 'No se encontró el ID de la liga.';
      this.cargando = false;
      return;
    }

    if (!this.equipoId) {
      this.errorMessage = 'No se encontró el ID del equipo.';
      this.cargando = false;
      return;
    }

    if (!this.jugadorId) {
      this.errorMessage = 'No se encontró el ID del jugador.';
      this.cargando = false;
      return;
    }

    const id = Number(this.jugadorId);

    this.servicioLigas.obtenerEstadisticasPorJugadorId(id).subscribe({
      next: (estadisticas) => {
        this.cargando = false;
        console.log(estadisticas);
        this.estadisticasJugador = estadisticas;
      },
      error: (error) => {
        console.error('Error cargando estadísticas:', error);
        this.errorMessage = 'No se pudo cargar las estadísticas del jugador.';
      }
    });

  }
}
