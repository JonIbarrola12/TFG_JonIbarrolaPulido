import { Component, OnInit, inject } from '@angular/core';
import { ServicioEquipos } from '../teams.service';

@Component({
  selector: 'app-team-detail',
  standalone: true,
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.css']
})
export class TeamDetailComponent implements OnInit {
  private readonly servicioEquipos = inject(ServicioEquipos);

  ngOnInit(): void {
    void this.servicioEquipos;
  }
}
