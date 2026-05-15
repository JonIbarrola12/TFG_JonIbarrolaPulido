import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';

import { ServicioPartidos } from '../matches.service';
import { ServicioLigas } from '../../leagues/leagues.service';

@Component({
  selector: 'app-matches-list',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './matches-list.component.html',
  styleUrls: ['./matches-list.component.css']
})
export class MatchesListComponent implements OnInit {

  private readonly partidosService = inject(ServicioPartidos);
  private readonly ligasService = inject(ServicioLigas);

  constructor(private cdr: ChangeDetectorRef) {}

  Object = Object;

  // DATA
  partidos: any[] = [];
  partidosFiltrados: any[] = [];

  equiposPorPais: { [pais: string]: any[] } = {};

  // UI
  cargando = true;
  errorMessage = '';

  // PAGINACIÓN
  paginaActual = 1;
  tamanoPagina = 10;

  // FILTROS
  fechaInicio: string | null = null;
  fechaFin: string | null = null;

  equiposSeleccionados: number[] = [];
  dropdownAbierto = false;

  // INIT
  ngOnInit(): void {
    this.cargarPartidos();
    this.cargarEquipos();
  }

  // CARGA PARTIDOS
  cargarPartidos() {
    this.cargando = true;

    this.partidosService.getPartidos().subscribe({
      next: (data: any[]) => {

        this.partidos = data.sort((a, b) =>
          new Date(a.Fecha).getTime() - new Date(b.Fecha).getTime()
        );

        this.partidosFiltrados = [...this.partidos];
        this.cargando = false;
      },
      error: () => {
        this.errorMessage = 'Error cargando partidos';
        this.cargando = false;
      }
    });
  }

  // CARGA EQUIPOS
  cargarEquipos() {
    this.partidosService.getEquipos().subscribe({
      next: (data) => {
        this.equiposPorPais = data.reduce((acc: any, eq: any) => {
          if (!acc[eq.Pais]) acc[eq.Pais] = [];
          acc[eq.Pais].push(eq);
          return acc;
        }, {});
      },
      error: () => {}
    });
  }

  // PAGINACIÓN
  get partidosPaginados() {
    const inicio = (this.paginaActual - 1) * this.tamanoPagina;
    return this.partidosFiltrados.slice(inicio, inicio + this.tamanoPagina);
  }

  cambiarPagina(pagina: number) {
    if (pagina < 1) return;

    const max = Math.ceil(this.partidosFiltrados.length / this.tamanoPagina);

    if (pagina > max) return;

    this.paginaActual = pagina;
  }

  // FILTROS
  aplicarFiltros() {

    let filtrados = [...this.partidos];

    // 📅 FECHAS
    if (this.fechaInicio && !this.fechaFin) {

      const dia = this.fechaInicio;

      filtrados = filtrados.filter(p =>
        new Date(p.Fecha).toISOString().split('T')[0] === dia
      );

    } else if (!this.fechaInicio && this.fechaFin) {

      const dia = this.fechaFin;

      filtrados = filtrados.filter(p =>
        new Date(p.Fecha).toISOString().split('T')[0] === dia
      );

    } else if (this.fechaInicio && this.fechaFin) {

      const inicio = new Date(this.fechaInicio + 'T00:00:00').getTime();
      const fin = new Date(this.fechaFin + 'T23:59:59').getTime();

      filtrados = filtrados.filter(p => {
        const fecha = new Date(p.Fecha).getTime();
        return fecha >= inicio && fecha <= fin;
      });
    }

    // ⚽ EQUIPOS
    if (this.equiposSeleccionados.length > 0) {
      filtrados = filtrados.filter(p =>
        this.equiposSeleccionados.includes(p.EquipoLocal?.EquipoId) ||
        this.equiposSeleccionados.includes(p.EquipoVisitante?.EquipoId)
      );
    }

    this.partidosFiltrados = filtrados;
    this.paginaActual = 1;
  }

  // RESET
  resetFiltros() {
    this.fechaInicio = null;
    this.fechaFin = null;
    this.equiposSeleccionados = [];

    this.partidosFiltrados = [...this.partidos];
    this.paginaActual = 1;
  }

  // DROPDOWN
  toggleDropdown() {
    this.dropdownAbierto = !this.dropdownAbierto;
  }

  toggleEquipo(id: number) {
    if (this.equiposSeleccionados.includes(id)) {
      this.equiposSeleccionados =
        this.equiposSeleccionados.filter(e => e !== id);
    } else {
      this.equiposSeleccionados.push(id);
    }
  }

  // UTIL
  private normalizarFecha(fecha: any): string {
    if (!fecha) return '';

    const d = new Date(fecha);

    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}
