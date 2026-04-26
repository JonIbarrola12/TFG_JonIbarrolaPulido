import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ServicioLigas } from '../leagues.service';

@Component({
  selector: 'app-league-players',
  imports: [RouterModule],
  templateUrl: './league-players.component.html',
  styleUrl: './league-players.component.css'
})

export class LeaguePlayersComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly servicioLigas = inject(ServicioLigas);

    ligaId: string | null = null;
    equipoId: string | null = null;
    equipo: any;
    jugadores: any[] = [];

    cargando = true;
    errorMessage = '';



  ngOnInit(): void {
    this.ligaId = this.route.snapshot.paramMap.get('id');
    this.equipoId = this.route.snapshot.paramMap.get('equipoId');

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

    const id = Number(this.equipoId);


    this.servicioLigas.obtenerEquipoPorEquipoId(id).subscribe({
      next: (equipo) => {
        this.cargando = false;
        this.equipo = equipo;

      },
      error: (error) => {
        console.error('Error cargando equipo:', error);
        this.errorMessage = 'No se pudo cargar la información del equipos.';
      }
    });

    this.servicioLigas.obtenerJugadoresPorEquipoId(id).subscribe({
      next: (jugadores) => {
        this.cargando = false;
        console.log(jugadores);
        this.jugadores = jugadores;

      },
      error: (error) => {
        console.error('Error cargando jugadores del equipo:', error);
        this.errorMessage = 'No se pudo cargar la información de los jugadores del equipo.';
      }
    });
  }
  formatHeight(value: any): string {
    if (!value) return '';

    const num = Number(value);

    // si ya viene en cm
    if (num > 3) return `${num} cm`;

    // si viene en metros (ej: 1.82)
    return `${(num * 100).toFixed(0)} cm`;
  }

  formatWeight(value: any): string {
    if (!value) return '';

    const str = String(value).toLowerCase();

    // si ya trae kg
    if (str.includes('kg')) return str;

    const num = Number(value);

    if (!isNaN(num)) return `${num} kg`;

    return value;
  }
}
