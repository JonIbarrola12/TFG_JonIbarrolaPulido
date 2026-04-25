import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ServicioLigas } from '../leagues.service';

@Component({
  selector: 'app-league-teams',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './league-teams.component.html',
  styleUrls: ['./league-teams.component.css']
})
export class LeagueTeamsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly servicioLigas = inject(ServicioLigas);

    ligaId: string | null = null;
    equipos: any[] = [];

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

    this.servicioLigas.obtenerEquiposPorLigaId(id).subscribe({
      next: (equipos) => {
        this.cargando = false;
        this.equipos = equipos;

      },
      error: (error) => {
        console.error('Error cargando equipos de liga:', error);
        this.errorMessage = 'No se pudo cargar la información de los equipos de la liga.';
      }
    });
  }
  
}
