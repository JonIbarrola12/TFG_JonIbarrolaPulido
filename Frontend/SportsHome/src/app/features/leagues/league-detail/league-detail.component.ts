import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';

import { ServicioLigas } from '../leagues.service';
import { Liga } from '../../../core/models/ligas.model';

@Component({
  selector: 'app-league-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './league-detail.component.html',
  styleUrls: ['./league-detail.component.css']
})
export class LeagueDetailComponent implements OnInit {

  private readonly servicioLigas = inject(ServicioLigas);
  private readonly route = inject(ActivatedRoute);

  ligaId: string | null = null;
  liga?: Liga;

  cargando = true;
  errorMessage = '';

  clasificacion: any[] = [];
  clasificacionVista: any[] = [];

  secciones: any[] = [];

  configuracionLiga = {
    champions: [] as number[],
    europa: [] as number[],
    conference: [] as number[],
    relegation: 3
  };

  ngOnInit(): void {

    this.ligaId = this.route.snapshot.paramMap.get('id');

    if (!this.ligaId) {
      this.errorMessage = 'No se encontró el ID de la liga.';
      this.cargando = false;
      return;
    }

    const id = Number(this.ligaId);

    // 🔹 Cargar liga
    this.servicioLigas.obtenerLigaPorId(id).subscribe({
      next: (liga) => {

        this.liga = liga;

        this.configurarClasificacion();

      },
      error: (error) => {
        console.error('Error cargando detalle de liga:', error);
        this.errorMessage = 'No se pudo cargar la información de la liga.';
      }
    });

    // 🔹 Cargar clasificación
    this.servicioLigas.obtenerClasificacionPorLigaId(id).subscribe({
      next: (clasificacion) => {

        this.clasificacion = clasificacion;

        this.clasificacionVista = clasificacion.map((item: any) => ({
          posicion: item.Posicion,
          nombre: item.Equipo?.Nombre,
          puntos: item.Puntos,
          jugados: item.Jugados,
          ganados: item.Ganados,
          empatados: item.Empatados,
          perdidos: item.Perdidos,
          golesFavor: item.GolesAFavor,
          golesContra: item.GolesEnContra,
          diferenciaGoles: item.GolesAFavor - item.GolesEnContra,
          logo: item.Equipo?.Logo
        }));

        this.cargando = false;

      },
      error: () => {
        this.errorMessage = 'No se pudo cargar la clasificación.';
        this.cargando = false;
      }
    });

    this.crearSecciones();
  }

  private configurarClasificacion(): void {

    switch (this.liga?.nombre) {

      case 'La Liga':
        this.configuracionLiga = {
          champions: [1, 2, 3, 4],
          europa: [5, 6],
          conference: [7],
          relegation: 3
        };
        break;

      case 'Premier League':
        this.configuracionLiga = {
          champions: [1, 2, 3, 4],
          europa: [5, 6],
          conference: [7],
          relegation: 3
        };
        break;

      case 'Serie A':
        this.configuracionLiga = {
          champions: [1, 2, 3, 4, 5],
          europa: [6],
          conference: [7],
          relegation: 3
        };
        break;

      case 'Bundesliga':
        this.configuracionLiga = {
          champions: [1, 2, 3, 4, 5],
          europa: [6],
          conference: [7],
          relegation: 3
        };
        break;

      case 'Ligue 1':
        this.configuracionLiga = {
          champions: [1, 2, 3],
          europa: [4, 5],
          conference: [6],
          relegation: 3
        };
        break;

      default:
        this.configuracionLiga = {
          champions: [1, 2, 3, 4],
          europa: [5],
          conference: [6],
          relegation: 3
        };
    }
  }

  private crearSecciones(): void {

    this.secciones = [
      {
        icono: '🛡️',
        titulo: 'Equipos',
        descripcion: 'Información detallada de equipos y sus plantillas.',
        ruta: `/ligas/${this.ligaId}/equipos`
      },
      {
        icono: '📅',
        titulo: 'Partidos',
        descripcion: 'Resultados, eventos y estadísticas por partido.',
        ruta: `/ligas/${this.ligaId}/partidos`
      },
      {
        icono: '📊',
        titulo: 'Estadísticas',
        descripcion: 'Estadísticas de la liga.',
        ruta: `/ligas/${this.ligaId}/estadisticas`
      }
    ];
  }

  mostrarClasificacion(): any[] {
    return this.clasificacionVista;
  }
}
