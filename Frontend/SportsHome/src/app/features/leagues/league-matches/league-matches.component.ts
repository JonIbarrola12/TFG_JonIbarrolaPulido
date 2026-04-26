import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ServicioLigas } from '../leagues.service';
import { OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Liga } from '../../../core/models/ligas.model';

@Component({
  selector: 'app-league-matches',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './league-matches.component.html',
  styleUrls: ['./league-matches.component.css']
})
export class LeagueMatchesComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly servicioLigas = inject(ServicioLigas)

  ligaId: string | null = null
  liga?: Liga;
  partidos: any[] = []
  cargando = true
  errorMessage = ''

  paginaActual = 1;
  tamanoPagina = 10;

  partidosPaginados: any[] = [];

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
        this.cargando = false;
      },
      error: (error) => {
        console.error('Error cargando detalle de liga:', error);
        this.errorMessage = 'No se pudo cargar la información de la liga.';
      }
    })
    this.servicioLigas.obtenerPartidosPorLigaId(id).subscribe({
      next: (partidos) => {
        this.cargando = false;
        this.partidos = partidos;
        this.actualizarPaginacion();
      },
      error: (error) => {
        console.error('Error cargando partidos de liga:', error);
        this.errorMessage = 'No se pudo cargar la información de los partidos de la liga.';
      }
    });
  }

  actualizarPaginacion(): void {
    const inicio = (this.paginaActual - 1) * this.tamanoPagina;
    const fin = inicio + this.tamanoPagina;

    this.partidosPaginados = this.partidos.slice(inicio, fin);
  }

  cambiarPagina(nuevaPagina: number): void {
  this.paginaActual = nuevaPagina;
  this.actualizarPaginacion();
}
}
