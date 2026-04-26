import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ServicioLigas } from '../leagues.service';
import { Liga } from '../../../core/models/ligas.model';

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
    estadisticas: any[] = [];

    cargando = true;
    errorMessage = '';



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
        this.cargando = false;
        this.liga = liga;
      },
      error: (error) => {
        console.error('Error cargando detalle de liga:', error);
        this.errorMessage = 'No se pudo cargar la información de la liga.';
      }
    });
  }
}
