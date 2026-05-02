import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ServicioLigas } from '../leagues.service';
import { Liga } from '../../../core/models/ligas.model';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-league-stadistics',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './league-stadistics.component.html',
  styleUrls: ['./league-stadistics.component.css']
})
export class LeagueStadisticsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly servicioLigas = inject(ServicioLigas);

  liga?: Liga;
  ligaId: string | null = null;
  cargando = true;
  errorMessage = '';

  topGoleadores: any[] = [];
  topAsistentes: any[] = [];
  topTarjetasAmarillas: any[] = [];
  topTarjetasRojas: any[] = [];
  topMinutos: any[] = [];

  estadisticasList: any[] = [];

  temporada = 2024;

  ngOnInit(): void {
    this.ligaId = this.route.snapshot.paramMap.get('id');

    if (!this.ligaId) {
      this.errorMessage = 'No se encontró el ID de la liga.';
      this.cargando = false;
      return;
    }

    const id = Number(this.ligaId);

    this.servicioLigas.obtenerLigaPorId(id).subscribe({
      next: (liga) => {
        this.liga = liga;
        this.cargarEstadisticas(id);
      },
      error: (error) => {
        console.error('Error cargando detalle de liga:', error);
        this.errorMessage = 'No se pudo cargar la información de la liga.';
        this.cargando = false;
      }
    });
  }

  cargarEstadisticas(id: number) {
    this.cargando = true;

    forkJoin({
      goleadores: this.servicioLigas.obtenerTopGoleadoresPorLigaId(id, this.temporada),
      asistentes: this.servicioLigas.obtenerTopAsistentesPorLigaId(id, this.temporada),
      amarillas: this.servicioLigas.obtenerTopTarjetasAmarillasPorLigaId(id, this.temporada),
      rojas: this.servicioLigas.obtenerTopTarjetasRojasPorLigaId(id, this.temporada),
      minutos: this.servicioLigas.obtenerTopMinutosPorLigaId(id, this.temporada)
    }).subscribe({
      next: (res) => {
        console.log(res);
        this.topGoleadores = res.goleadores;
        this.topAsistentes = res.asistentes;
        this.topTarjetasAmarillas = res.amarillas;
        this.topTarjetasRojas = res.rojas;
        this.topMinutos = res.minutos;

        this.actualizarLista();
        this.cargando = false;
      },
      error: () => {
        this.errorMessage = 'Error cargando estadísticas';
        this.cargando = false;
      }
    });
  }

  actualizarLista() {
    this.estadisticasList = [
      {
        titulo: 'Top Goleadores',
        datos: this.topGoleadores,
        columna: 'Goles',
        campo: 'Goles'
      },
      {
        titulo: 'Top Asistentes',
        datos: this.topAsistentes,
        columna: 'Asistencias',
        campo: 'Asistencias'
      },
      {
        titulo: 'Tarjetas Amarillas',
        datos: this.topTarjetasAmarillas,
        columna: 'Amarillas',
        campo: 'TarjetasAmarillas'
      },
      {
        titulo: 'Tarjetas Rojas',
        datos: this.topTarjetasRojas,
        columna: 'Rojas',
        campo: 'TarjetasRojas'
      },
      {
        titulo: 'Minutos',
        datos: this.topMinutos,
        columna: 'Minutos',
        campo: 'Minutos'
      }
    ];
  }
}
