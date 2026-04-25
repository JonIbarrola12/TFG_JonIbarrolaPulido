import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Component, OnInit, inject } from '@angular/core';
import { ServicioLigas } from '../leagues.service';
import { Liga } from '../../../core/models/ligas.model';

@Component({
  selector: 'app-leagues-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './leagues-list.component.html',
  styleUrls: ['./leagues-list.component.css']
})
export class LeaguesListComponent implements OnInit {
  private readonly servicioLigas = inject(ServicioLigas);

  ligas: Liga[] = [];
  errorMessage = '';

  ngOnInit(): void {
    this.servicioLigas.obtenerLigas().subscribe({
      next: (ligas) => {
        this.ligas = ligas;
      },
      error: (error) => {
        console.error('Error cargando ligas:', error);
        this.errorMessage = 'No se pudieron cargar las ligas. Revisa la consola para más detalles.';
      }
    });
  }
}
